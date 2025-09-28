using System.Runtime.CompilerServices;
using TreeVal.Media;
using TreeVal.Primitives;
using TreeVal.Scaffolding;
using TreeVal.Stage.Eval.Where;

namespace TreeVal.Stage.Eval.Equals;

public static class ScaffoldingExtensions
{
    public static IVisitorBuilder Equals<T>(
        this IVisitorBuilderFactory factory,
        T left,
        Func<Node, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new VisitorBuilder(factory);
        builder.AddCondition(
            new WhereCondition($"Equals({left}, {getRightExpression})", e => Equals(left, getRight(e))));
        return builder;
    }

    public static IVisitorBuilder Equals(
        this IVisitorBuilderFactory factory,
        string? left,
        Func<Node, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new VisitorBuilder(factory);
        builder.AddCondition(
            new WhereCondition($"{getRightExpression} == \"{left}\"", e => left == getRight(e)));
        return builder;
    }

    public static IVisitorBuilder Equals(
        this IVisitorBuilderFactory factory,
        Type? left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new VisitorBuilder(factory);
        builder.AddCondition(
            new WhereCondition($"{getRightExpression} == typeof({left})", e => left == getRight(e)));
        return builder;
    }

    public static TBuilder Equals<TBuilder>(
        this TBuilder builder,
        Type? left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TBuilder : IVisitorBuilder
    {
        builder.AddCondition(
            new WhereCondition($"{getRightExpression} == typeof({left})", e => left == getRight(e)));
        return builder;
    }

    public static TBuilder Equals<TBuilder>(
        this TBuilder builder,
        EType left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TBuilder : IVisitorBuilder
    {
        builder.AddCondition(
            new WhereCondition(
                $"EType.AreEqual({left}, {getRightExpression})",
                e => EType.AreEqual(left, getRight(e))));
        return builder;
    }

    public static IVisitorBuilder<TNode> Equals<TNode, T>(
        this IVisitorBuilder<TNode> builder,
        T left,
        Func<TNode, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(
            new WhereCondition<TNode>(
                $"Equals({left}, {getRightExpression})",
                t => Equals(left, getRight(t))));
        return builder;
    }

    public static IVisitorBuilder<T> Equals<T>(
        this IVisitorBuilder<T> builder,
        string? left,
        Func<T, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(
            new WhereCondition<T>(
                $"{getRightExpression} == \"{left}\"",
                t => left == getRight(t)));
        return builder;
    }

    public static IVisitorBuilder<TExpression> Equals<TExpression>(
        this IVisitorBuilder<TExpression> builder,
        Type? left,
        Func<TExpression, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(
            new WhereCondition<TExpression>(
                $"{getRightExpression} == typeof({left})",
                t => left == getRight(t)));
        return builder;
    }

    public static IVisitorBuilder<TExpression> Equals<TExpression, T>(
        this IVisitorBuilder<TExpression> builder,
        EValue<T> left,
        Func<TExpression, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(
            new WhereCondition<TExpression>(
                $"EValue.AreEqual({left}, {getRightExpression})",
                t => EValue.AreEqual(left, getRight(t))));
        return builder;
    }
}
