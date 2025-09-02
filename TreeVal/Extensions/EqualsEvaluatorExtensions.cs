using System.Runtime.CompilerServices;
using TreeVal.Condition;
using TreeVal.Media;

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
        this TBuilder builder,
        Type? left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        builder.AddCondition(
            new WhereCondition($"typeof({left}) == {getRightExpression}", e => left == getRight(e)));
        return builder;
    }

    public static TBuilder Equals<TBuilder>(
        this TBuilder builder,
        EType left,
        Func<Node, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        builder.AddCondition(new WhereCondition(
            $"EType.AreEqual({left}, {getRightExpression})", e => EType.AreEqual(left, getRight(e))));
        return builder;
    }

    public static IEvaluatorBuilder<TNode> Equals<TNode, T>(
        this IEvaluatorBuilder<TNode> builder,
        T left,
        Func<TNode, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition<TNode>(
            $"Equals({left}, {getRightExpression})", t => Equals(left, getRight(t))));
        return builder;
    }

    public static IEvaluatorBuilder<T> Equals<T>(
        this IEvaluatorBuilder<T> builder,
        string? left,
        Func<T, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition<T>(
            $"\"{left}\" == {getRightExpression}", t => left == getRight(t)));
        return builder;
    }

    public static IEvaluatorBuilder<TExpression> Equals<TExpression>(
        this IEvaluatorBuilder<TExpression> builder,
        Type? left,
        Func<TExpression, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition<TExpression>(
            $"typeof({left}) == {getRightExpression}", t => left == getRight(t)));
        return builder;
    }

    public static IEvaluatorBuilder<TExpression> Equals<TExpression, T>(
        this IEvaluatorBuilder<TExpression> builder,
        EValue<T> left,
        Func<TExpression, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition<TExpression>(
            $"EValue.AreEqual({left}, {getRightExpression})", t => EValue.AreEqual(left, getRight(t))));
        return builder;
    }
}
