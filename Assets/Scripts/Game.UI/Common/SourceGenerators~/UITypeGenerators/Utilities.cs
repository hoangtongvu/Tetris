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
                //if (attribute.ArgumentList?.Arguments.Count != 2) continue;

                return true;
            }
        }

        return false;
    }

    public static ConcreteUIInfo GetConcreteUIInfo(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        GetUITypeInfo(classDeclaration, out var uiTypeName, out var fallbackUnderlyingValue);

        return new()
        {
            ConcreteUICtrlName = classDeclaration.Identifier.ToString(),
            ConcreteUICtrlNamespace = GetNamespace(classDeclaration),
            UITypeName = uiTypeName,
            FallbackUnderlyingValue = fallbackUnderlyingValue,
        };
    }

    private static bool GetUITypeInfo(
        ClassDeclarationSyntax classDeclaration,
        out string uiTypeName,
        out int fallbackUnderlyingValue)
    {
        uiTypeName = null;
        fallbackUnderlyingValue = default;

        int attributeListCount = classDeclaration.AttributeLists.Count;

        for (int i = 0; i < attributeListCount; i++)
        {
            var attributes = classDeclaration.AttributeLists[i].Attributes;
            int attributeCount = attributes.Count;

            for (int j = 0; j < attributeCount; j++)
            {
                var attribute = attributes[j];
                string attributeName = attribute.Name.ToString();

                var arguments = attribute.ArgumentList.Arguments;
                int argumentCount = arguments.Count;

                if (!genertateUITypeAttributeIdentifiers.Contains(attributeName))
                    //|| arguments.Count != 2)
                {
                    continue;
                }

                if (arguments[0].Expression is not LiteralExpressionSyntax stringLiteral
                    || !stringLiteral.IsKind(SyntaxKind.StringLiteralExpression))
                {
                    return false;
                }

                if (argumentCount == 2)
                {
                    if (arguments[1].Expression is not LiteralExpressionSyntax intLiteral
                        || !intLiteral.IsKind(SyntaxKind.NumericLiteralExpression)
                        || intLiteral.Token.Value is not int value)
                    {
                        return false;
                    }

                    fallbackUnderlyingValue = value;
                }
                else
                {
                    fallbackUnderlyingValue = 0;
                }

                uiTypeName = stringLiteral.Token.ValueText;

                return true;
            }
        }

        return false;
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