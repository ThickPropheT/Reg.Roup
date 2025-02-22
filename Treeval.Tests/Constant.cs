using System.Linq.Expressions;

namespace Treeval.Tests;

[TestFixture]
public class Constant
{
    private static readonly Expression InvalidExpression =
        Expression.Add(Expression.Constant(1), Expression.Constant(1));

    private static readonly Expression ValidExpression =
        Expression.Constant(69);

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.Constant()),
        ExpressionTreeEvaluator.Create(node => node.Constant(69)),
        ExpressionTreeEvaluator.Create(node => node.Constant<int>())
    ];

    [TestCaseSource(nameof(Evaluators))]
    public void ThrowsOnInvalidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(InvalidExpression), Throws.Exception);
    }

    [TestCaseSource(nameof(Evaluators))]
    public void DoesNotThrowOnValidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(ValidExpression), Throws.Nothing);
    }
}
