using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplicitLayers.Analyzers.Extensions
{
    internal static class SemanticModelExtensions
    {
        /// <summary>
        /// Get all type declarations with a specific attribute
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static List<TypeDeclarationSyntax> GetNodesOfAttributedTypes(this SemanticModel semanticModel, string attributeName)
        {
            var typeDeclarations = semanticModel.SyntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>();
            var symbolList = new List<TypeDeclarationSyntax>();

            foreach (var declaration in typeDeclarations)
            {
                foreach (var attributeList in declaration.AttributeLists)
                {
                    if (attributeList.Attributes.Any(a => (a.Name as IdentifierNameSyntax)?.Identifier.Text == attributeName))
                    {
                        symbolList.Add(declaration);
                        break;
                    }
                }
            }
            return symbolList;
        }
    }
}
