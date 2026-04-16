using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace UITypeGenerators;

public static class Utilities
{
    private static readonly HashSet<string> genertateUITypeAttributeIdentifiers = new()
    {
        GenerateUITypeAttributeContants.NAME,
        GenerateUITypeAttributeContants.IDENTIFIER,
        $"{GenerateUITypeAttributeContants.NAME}Attribute",
        $"{GenerateUITypeAttributeContants.IDENTIFIER}Attribute",
    };

    public static bool IsTargetNode(SyntaxNode syntaxNode)
    {
        return syntaxNode is ClassDeclarationSyntax classDeclaration
            && IsPartialClass(classDeclaration)
            && HasGenertateUITypeAttribute(classDeclaration);
    }

    private static bool HasGenertateUITypeAttribute(ClassDeclarationSyntax classDeclaration)
    {
        int attributeListCount = classDeclaration.AttributeLists.Count;

        for (int i = 0; i < attributeListCount; i++)
        {
            var attributes = classDeclaration.AttributeLists[i].Attributes;
            int attributeCount = attributes.Count;

            for (int j = 0; j < attributeCount; j++)
            {
                var attribute = attributes[j];
                string attributeName = attribute.Name.ToString();

                if (!genertateUITypeAttributeIdentifiers.Contains(attributeName)) continue;
                if (attribute.ArgumentList?.Arguments.Count != 1) continue;

                return true;
            }
        }

        return false;
    }

    public static ConcreteUIInfo GetConcreteUIInfo(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new()
        {
            ConcreteUICtrlName = classDeclaration.Identifier.ToString(),
            ConcreteUICtrlNamespace = GetNamespace(classDeclaration),
            UITypeName = GetUITypeName(classDeclaration),
        };
    }

    private static string GetUITypeName(ClassDeclarationSyntax classDeclaration)
    {
        int attributeListCount = classDeclaration.AttributeLists.Count;

        for (int i = 0; i < attributeListCount; i++)
        {
            var attributes = classDeclaration.AttributeLists[i].Attributes;
            int attributeCount = attributes.Count;

            for (int j = 0; j < attributeCount; j++)
            {
                var attribute = attributes[j];
                string attributeName = attribute.Name.ToString();

                if (!genertateUITypeAttributeIdentifiers.Contains(attributeName)
                    || attribute.ArgumentList?.Arguments.Count != 1) continue;

                if (attribute.ArgumentList.Arguments[0].Expression is not LiteralExpressionSyntax literalExpression)
                    return null;

                if (!literalExpression.IsKind(SyntaxKind.StringLiteralExpression))
                    return null;

                return literalExpression.Token.ValueText;
            }
        }

        return null;
    }

    public static bool IsPartialClass(ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PartialKeyword));
    }

    public static string GetNamespace(SyntaxNode syntaxNode)
    {
        SyntaxNode parent = syntaxNode.Parent;

        while (parent != null)
        {
            switch (parent)
            {
                case NamespaceDeclarationSyntax namespaceDecl:
                    return namespaceDecl.Name.ToString();
                case FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDecl:
                    return fileScopedNamespaceDecl.Name.ToString();
            }

            parent = parent.Parent;
        }

        return null;
    }
}