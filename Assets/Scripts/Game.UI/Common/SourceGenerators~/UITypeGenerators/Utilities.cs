using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using static UITypeGenerators.AttributeConstants;

namespace UITypeGenerators;

public static class Utilities
{
    public static BaseTypeInfo? GetBaseTypeInfo(GeneratorAttributeSyntaxContext ctx)
    {
        if (ctx.TargetSymbol is not INamedTypeSymbol classSymbol)
            return null;

        var attribute = ctx.Attributes.FirstOrDefault(a =>
            a.AttributeClass?.Name == AttributeName);

        if (attribute is null)
            return null;

        // enumName is the first constructor argument
        var enumName = attribute.ConstructorArguments.Length > 0
            ? attribute.ConstructorArguments[0].Value as string
            : null;

        if (string.IsNullOrWhiteSpace(enumName))
            return null;

        var namespaceName = classSymbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : classSymbol.ContainingNamespace.ToDisplayString();

        return new BaseTypeInfo(
            classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            classSymbol.ToDisplayString(),
            namespaceName,
            enumName!);
    }

    public static ImmutableArray<INamedTypeSymbol> CollectAllClasses(Compilation compilation)
    {
        var builder = ImmutableArray.CreateBuilder<INamedTypeSymbol>();
        CollectClasses(compilation.GlobalNamespace, builder);
        return builder.ToImmutable();
    }

    private static void CollectClasses(
        INamespaceOrTypeSymbol symbol,
        ImmutableArray<INamedTypeSymbol>.Builder builder)
    {
        foreach (var member in symbol.GetMembers())
        {
            switch (member)
            {
                case INamespaceSymbol ns:
                    CollectClasses(ns, builder);
                    break;
                case INamedTypeSymbol { TypeKind: TypeKind.Class } cls:
                    builder.Add(cls);
                    CollectClasses(cls, builder); // support nested types
                    break;
            }
        }
    }

    /// <summary>
    /// Walks the base-type chain of <paramref name="symbol"/> looking for
    /// a type whose fully-qualified name matches <paramref name="targetFqn"/>.
    /// </summary>
    public static bool IsDirectOrIndirectSubclass(INamedTypeSymbol symbol, string targetFqn)
    {
        var current = symbol.BaseType;
        while (current is not null)
        {
            if (current.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == targetFqn)
                return true;
            current = current.BaseType;
        }
        return false;
    }

    /// <summary>
    /// Returns the value from <c>[UITypeValue(n)]</c> if present, otherwise null.
    /// </summary>
    public static int? GetExplicitValue(INamedTypeSymbol symbol)
    {
        foreach (var attr in symbol.GetAttributes())
        {
            if (attr.AttributeClass?.Name == ValueAttributeName &&
                attr.ConstructorArguments.Length == 1 &&
                attr.ConstructorArguments[0].Value is int value)
            {
                return value;
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

    public static int StableHash(string text)
    {
        unchecked
        {
            const uint offset = 2166136261;
            const uint prime = 16777619;

            uint hash = offset;

            foreach (char c in text)
            {
                hash ^= c;
                hash *= prime;
            }

            return (int)hash;
        }
    }
}