using System.Linq.Expressions;
using TreeVal.Extensions;

namespace TreeVal.Tests;

[TestFixture]
public class OneOf
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Break(Expression.Label()),
        Expression.Constant("1"),
        Expression.Constant(2),
        Expression.Subtract(Expression.Constant(1), Expression.Constant(1)),
        Expression.Add(Expression.Constant(2), Expression.Constant(2)),
        Expression.Add(Expression.Add(Expression.Constant(1), Expression.Constant(1)), Expression.Constant(1))
    ];

    private static readonly Expression[] ValidExpressions =
    [
        Expression.Constant(1),
        Expression.Constant("2"),
        Expression.Add(Expression.Constant(1), Expression.Constant(1))
    ];

    private static readonly ExpressionTreeEvaluator Evaluator = ExpressionTreeEvaluator.Create(node =>
        node.OneOf(
            node.Constant(1),
            node.Constant("2"),
            node
                .OfType<BinaryExpression>(ExpressionType.Add)
                .HavingChildren(
                    node.Constant(1),
                    node.Constant(1)
                )
        )
    );

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
