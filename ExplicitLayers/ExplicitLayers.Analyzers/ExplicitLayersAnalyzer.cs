using ExplicitLayers.Analyzers.Config;
using ExplicitLayers.Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ExplicitLayers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ExplicitLayersAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ExplicitLayers";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Architecture";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }
        private static Configuration Configuration { get; set; }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSemanticModelAction(AnalyzeSemantics);
        }

        private static void AnalyzeSemantics(SemanticModelAnalysisContext context)
        {
            Configuration = Configuration.ReadLayerConfiguration(context) ?? Configuration.Default;

            // Get all type defining nodes (classes, interfaces, structs) in a file
            var typeNodes = context.SemanticModel.GetNodesOfAttributedTypes(Constants.AttributeName).Select(n => new { node = n, symbol = context.SemanticModel.GetDeclaredSymbol(n) });
            var stree = context.SemanticModel.SyntaxTree;

            // Ways to reference another type in code
            var targetExpressions = new Type[]
            {
                typeof(MemberAccessExpressionSyntax),
                typeof(ObjectCreationExpressionSyntax),
                typeof(ParameterSyntax)
            };
            // Check if there are any layers defined with paths that this file matches 
            var pathLayer = Configuration.GetLayerNameByPath(stree.FilePath);

            foreach (var currentTypeNode in typeNodes)
            {
                var currentTypeLayer = GetLayerNameFromSymbol(currentTypeNode.symbol) ?? pathLayer;
                // Get all references to types in code
                var referenceExpressions = currentTypeNode.node.DescendantNodes().Where(dn => targetExpressions.Contains(dn.GetType()));

                // Check mentions in code
                foreach (var reference in referenceExpressions)
                {
                    ISymbol symbol = GetSymbolFromExpression(context.SemanticModel, reference);
                    var expressionLayer = GetLayerName(symbol);
                    if (expressionLayer == null)
                        continue; // If there's no layer resolved for referenced type, nothing to do

                    if (currentTypeLayer != expressionLayer && !Configuration.IsDependencyAllowed(currentTypeLayer, expressionLayer))
                    {
                        var l = Location.Create(context.SemanticModel.SyntaxTree, reference.Span);
                        context.ReportDiagnostic(Diagnostic.Create(Rule, l, symbol.ContainingType.Name, expressionLayer, currentTypeNode.node.Identifier.ValueText, currentTypeLayer));
                    }
                }

                // Check inheritances of the type
                foreach (var currentTypeParent in currentTypeNode.symbol.AllInterfaces.Concat(new INamedTypeSymbol[] { currentTypeNode.symbol.BaseType }))
                {
                    var parentLayer = GetLayerName(currentTypeParent);
                    if (parentLayer == null)
                        continue;

                    if (currentTypeLayer != parentLayer && !Configuration.IsDependencyAllowed(currentTypeLayer, parentLayer))
                    {
                        var l = Location.Create(context.SemanticModel.SyntaxTree, currentTypeNode.node.BaseList.Span);
                        context.ReportDiagnostic(Diagnostic.Create(Rule, l, currentTypeParent.Name, parentLayer, currentTypeNode.node.Identifier.ValueText, currentTypeLayer));
                    }
                }
            }
        }

        private static ISymbol GetSymbolFromExpression(SemanticModel semanticModel, SyntaxNode syntaxNode)
        {
            var symbolInfo = semanticModel.GetSymbolInfo(syntaxNode);
            var symbol = symbolInfo.Symbol ?? symbolInfo.CandidateSymbols.FirstOrDefault();
            return symbol;
        }

        private static string GetLayerName(ISymbol symbol)
        {
            if (symbol == null)
                return null;
            var expressionLayer = GetLayerNameFromSymbol(symbol);
            if (expressionLayer == null)
            {
                // If referenced type isn't attributed, try to get layer by path from Configuration
                string expressionPathLayer = null;
                foreach (var location in symbol.Locations)
                {
                    if (location?.SourceTree?.FilePath == null)
                        continue;
                    expressionPathLayer = Configuration.GetLayerNameByPath(location.SourceTree.FilePath);
                    if (expressionPathLayer != null)
                        break;
                }
                if (expressionPathLayer == null)
                    return null;
                expressionLayer = expressionPathLayer;
            }
            return expressionLayer;
        }

        private static string GetLayerNameFromSymbol(ISymbol symbol)
        {
            ISymbol symbolToCheck = symbol.ContainingType;
            if (symbol is ITypeSymbol)
                symbolToCheck = symbol;
            return symbolToCheck?.GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == $"{Constants.AttributeName}Attribute")?.ConstructorArguments.FirstOrDefault().Value?.ToString();
        }



    }

}
