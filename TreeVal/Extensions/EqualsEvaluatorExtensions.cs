using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal.Extensions;

// TODO try to de-dupe some of these method bodies
public static class EqualsEvaluatorExtensions
{
    public static IEvaluatorBuilder Equals<T>(
        this IVisitorNodeFactory _,
        Func<Expression, T> getLeft,
        T right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"Equals({getLeftExpression}, {right})", e => Equals(getLeft(e), right)));
        return builder;
    }

    public static IEvaluatorBuilder Equals(
        this IVisitorNodeFactory _,
        Func<Expression, string> getLeft,
        string right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"{getLeftExpression} == \"{right}\"", e => getLeft(e) == right));
        return builder;
    }

    public static IEvaluatorBuilder Equals(
        this IVisitorNodeFactory _,
        Func<Expression, Type> getLeft,
        Type right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(
            new WhereCondition($"{getLeftExpression} == typeof({right})", e => getLeft(e) == right));
        return builder;
    }

    public static TBuilder Equals<TBuilder, T>(
        this TBuilder node,
        Func<Expression, T> getLeft,
        T right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(
            new WhereCondition($"Equals({getLeftExpression}, {right})", e => Equals(getLeft(e), right)));
        return node;
    }

    public static TBuilder Equals<TBuilder>(
        this TBuilder node,
        Func<Expression, string> getLeft,
        string right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(
            new WhereCondition($"{getLeftExpression} == \"{right}\"", e => getLeft(e) == right));
        return node;
    }

    public static TBuilder Equals<TBuilder>(
        this TBuilder node,
        Func<Expression, Type> getLeft,
        Type right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(
            new WhereCondition($"{getLeftExpression} == typeof({right})", e => getLeft(e) == right));
        return node;
    }

    public static IEvaluatorBuilder<TExpression> Equals<TExpression, T>(
        this IEvaluatorBuilder<TExpression> node,
        Func<TExpression, T> getLeft,
        T right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        node.AddCondition(new WhereCondition($"Equals({getLeftExpression}, {right})", e =>
        {
            if (e is not TExpression t)
            {
                throw UnmetPreconditionException.WrongExpressionType<TExpression>(e);
            }

            return Equals(getLeft(t), right);
        }));

        return node;
    }
    
    public static IEvaluatorBuilder<TExpression> Equals<TExpression>(
        this IEvaluatorBuilder<TExpression> node,
        Func<TExpression, string> getLeft,
        string right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        node.AddCondition(new WhereCondition($"{getLeftExpression} == \"{right}\"", e =>
        {
            if (e is not TExpression t)
            {
                throw UnmetPreconditionException.WrongExpressionType<TExpression>(e);
            }

            return getLeft(t) == right;
        }));

        return node;
    }
    
    public static IEvaluatorBuilder<TExpression> Equals<TExpression>(
        this IEvaluatorBuilder<TExpression> node,
        Func<TExpression, Type> getLeft,
        Type right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        node.AddCondition(new WhereCondition($"{getLeftExpression} == typeof({right})", e =>
        {
            if (e is not TExpression t)
            {
                throw UnmetPreconditionException.WrongExpressionType<TExpression>(e);
            }

            return getLeft(t) == right;
        }));

        return node;
    }
}
