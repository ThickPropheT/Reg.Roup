using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal.Extensions;

public static class EqualsEvaluatorExtensions
{
    public static IEvaluatorBuilder Equals<T>(
        this IVisitorNodeFactory _,
        T left,
        Func<Node, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"Equals({left}, {getRightExpression})", e => Equals(left, getRight(e))));
        return builder;
    }

    public static IEvaluatorBuilder Equals(
        this IVisitorNodeFactory _,
        string? left,
        Func<Node, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"\"{left}\" == {getRightExpression}", e => left == getRight(e)));
        return builder;
    }

    public static IEvaluatorBuilder Equals(
        this IVisitorNodeFactory _,
        Type? left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"typeof({left}) == {getRightExpression}", e => left == getRight(e)));
        return builder;
    }

    public static TBuilder Equals<TBuilder>(
        this TBuilder node,
        Type? left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(
            new WhereCondition($"typeof({left}) == {getRightExpression}", e => left == getRight(e)));
        return node;
    }

    public static TBuilder Equals<TBuilder>(
        this TBuilder node,
        EType left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(new WhereCondition(
            $"EType.AreEqual({left}, {getRightExpression})", e => EType.AreEqual(left, getRight(e))));
        return node;
    }

    public static IEvaluatorBuilder<TNode> Equals<TNode, T>(
        this IEvaluatorBuilder<TNode> node,
        T left,
        Func<TNode, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(new WhereCondition<TNode>(
            $"Equals({left}, {getRightExpression})", t => Equals(left, getRight(t))));
        return node;
    }

    public static IEvaluatorBuilder<T> Equals<T>(
        this IEvaluatorBuilder<T> node,
        string? left,
        Func<T, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(new WhereCondition<T>(
            $"\"{left}\" == {getRightExpression}", t => left == getRight(t)));
        return node;
    }

    public static IEvaluatorBuilder<TExpression> Equals<TExpression>(
        this IEvaluatorBuilder<TExpression> node,
        Type? left,
        Func<TExpression, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(new WhereCondition<TExpression>(
            $"typeof({left}) == {getRightExpression}", t => left == getRight(t)));
        return node;
    }

    public static IEvaluatorBuilder<TExpression> Equals<TExpression, T>(
        this IEvaluatorBuilder<TExpression> node,
        EValue<T> left,
        Func<TExpression, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(new WhereCondition<TExpression>(
            $"EValue.AreEqual({left}, {getRightExpression})", t => EValue.AreEqual(left, getRight(t))));
        return node;
    }
}
