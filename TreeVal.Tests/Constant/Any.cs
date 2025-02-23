using System.Linq.Expressions;

namespace TreeVal.Tests.Constant;

[TestFixture]
public class Any
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Convert(Expression.Constant(1), typeof(short)),
        Expression.Return(Expression.Label()),
        Expression.Add(Expression.Constant(1), Expression.Constant(1)),
    ];

    private static readonly Expression[] ValidExpressions =
    [
        Expression.Constant(""),
        Expression.Constant(70),
        Expression.Constant(null, typeof(short?))
    ];

    private static readonly ExpressionTreeEvaluator Evaluator = ExpressionTreeEvaluator.Create(node => node.Constant());

    [TestCaseSource(nameof(InvalidExpressions))]
    public void ThrowsOnInvalidSchemas(Expression invalidExpression)
    {
        Assert.That(() => Evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }

    [TestCaseSource(nameof(ValidExpressions))]
    public void DoesNotThrowOnValidSchemas(Expression validExpression)
    {
        Assert.That(() => Evaluator.Evaluate(validExpression), Throws.Nothing);
    }
}
