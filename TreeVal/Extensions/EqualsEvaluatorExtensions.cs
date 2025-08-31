using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal.Extensions;

// TODO try to de-dupe some of these method bodies
public static class EqualsEvaluatorExtensions
{
    public static IEvaluatorBuilder<Expression> Equals<T>(
        this IVisitorNodeFactory _,
        T left,
        Func<Expression, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"Equals({left}, {getRightExpression})", e => Equals(left, getRight(e))));
        return builder;
    }

    public static IEvaluatorBuilder<Expression> Equals(
        this IVisitorNodeFactory _,
        string? left,
        Func<Expression, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"\"{left}\" == {getRightExpression}", e => left == getRight(e)));
        return builder;
    }

    public static IEvaluatorBuilder<Expression> Equals(
        this IVisitorNodeFactory _,
        Type? left,
        Func<Expression, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"typeof({left}) == {getRightExpression}", e => left == getRight(e)));
        return builder;
    }

    public static IEvaluatorBuilder<Expression> Equals<T>(
        this IEvaluatorBuilder<Expression> node,
        T left,
        Func<Expression, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(
            new WhereCondition($"Equals({left}, {getRightExpression})", e => Equals(left, getRight(e))));
        return node;
    }

    public static IEvaluatorBuilder<Expression> Equals(
        this IEvaluatorBuilder<Expression> node,
        string? left,
        Func<Expression, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(
            new WhereCondition($"\"{left}\" == {getRightExpression}", e => left == getRight(e)));
        return node;
    }

    public static IEvaluatorBuilder<Expression> Equals(
        this IEvaluatorBuilder<Expression> node,
        Type? left,
        Func<Expression, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(
            new WhereCondition($"typeof({left}) == {getRightExpression}", e => left == getRight(e)));
        return node;
    }

    public static IEvaluatorBuilder<Expression> Equals(
        this IEvaluatorBuilder<Expression> node,
        EType left,
        Func<Expression, Type?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(new WhereCondition(
            $"EType.AreEqual({left}, {getRightExpression})", e => EType.AreEqual(left, getRight(e))));
        return node;
    }

    public static IEvaluatorBuilder<TExpression> Equals<TExpression, T>(
        this IEvaluatorBuilder<TExpression> node,
        T left,
        Func<TExpression, object?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TExpression : Expression
    {
        node.AddCondition(new WhereCondition<TExpression>(
            $"Equals({left}, {getRightExpression})", t => Equals(left, getRight(t))));
        return node;
    }

    public static IEvaluatorBuilder<TExpression> Equals<TExpression>(
        this IEvaluatorBuilder<TExpression> node,
        string? left,
        Func<TExpression, string?> getRight,
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TExpression : Expression
    {
        node.AddCondition(new WhereCondition<TExpression>(
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
        where TExpression : Expression
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
        where TExpression : Expression
    {
        node.AddCondition(new WhereCondition<TExpression>(
            $"EValue.AreEqual({left}, {getRightExpression})", t => EValue.AreEqual(left, getRight(t))));
        return node;
    }
}
