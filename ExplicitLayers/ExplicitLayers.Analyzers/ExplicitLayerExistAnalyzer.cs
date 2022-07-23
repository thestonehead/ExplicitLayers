using ExplicitLayers.Analyzers.Config;
using ExplicitLayers.Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace ExplicitLayers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ExplicitLayerExistAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ExplicitLayerExist";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ExistsAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ExistsAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ExistsAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Architecture";
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSemanticModelAction(Analyze);
        }

        private static void Analyze(SemanticModelAnalysisContext context)
        {
            var configuration = Configuration.ReadLayerConfiguration(context) ?? Configuration.Default;

            var typeNodes = context.SemanticModel.GetNodesOfAttributedTypes(Constants.AttributeName);
            foreach (var typeNode in typeNodes)
            {
                var symbol = context.SemanticModel.GetDeclaredSymbol(typeNode);
                var attribute = symbol.GetAttributes().First(a => a.AttributeClass.Name == $"{Constants.AttributeName}Attribute");
                var argument = attribute.ConstructorArguments.FirstOrDefault().Value.ToString();
                if (!configuration.LayerConfigurations.Any(lc => lc.Name == argument))
                {
                    var attributeSyntax = typeNode.AttributeLists.SelectMany(al => al.Attributes).First(a => (a.Name as IdentifierNameSyntax)?.Identifier.Text == Constants.AttributeName);
                    var l = Location.Create(context.SemanticModel.SyntaxTree, attributeSyntax.Span);
                    context.ReportDiagnostic(Diagnostic.Create(Rule, l, argument));
                }
            }
        }
    }
}
