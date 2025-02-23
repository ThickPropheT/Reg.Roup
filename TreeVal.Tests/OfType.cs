using System.Linq.Expressions;

namespace TreeVal.Tests;

[TestFixtureSource(nameof(InvalidExpressions))]
public class OfType
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Break(Expression.Label()),
        Expression.Constant(1),
        Expression.New(typeof(object).GetConstructor(Type.EmptyTypes)!),
        Expression.Parameter(typeof(int), "index"),
        Expression.Rethrow(),
        Expression.DebugInfo(Expression.SymbolDocument("my.file"), 1, 1, 2, 2),
        Expression.Add(Expression.Constant(1), Expression.Constant(1))
    ];

    private readonly Expression _invalidExpression;
    private static readonly Expression ValidExpression = Expression.Convert(Expression.Constant(1), typeof(short));

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        // @formatter:off
        ExpressionTreeEvaluator.Create(node => node.OfType(ExpressionType.Convert).HavingChildren([node.AnyOne()])),
        ExpressionTreeEvaluator.Create(node => node.OfType<UnaryExpression>().HavingChildren(node.AnyOne())),
        ExpressionTreeEvaluator.Create(node => node.OfType<UnaryExpression>(ExpressionType.Convert).HavingChildren(node.AnyOne()))
        // @formatter:on
    ];

    public OfType(Expression invalidExpression)
    {
        _invalidExpression = invalidExpression;
    }

    [TestCaseSource(nameof(Evaluators))]
    public void ThrowsOnInvalidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(_invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }

    [TestCaseSource(nameof(Evaluators))]
    public void DoesNotThrowOnValidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(ValidExpression), Throws.Nothing);
    }
}
