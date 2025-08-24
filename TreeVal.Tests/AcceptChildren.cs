using System.Linq.Expressions;
using TreeVal.Extensions;

namespace TreeVal.Tests;

[TestFixture]
public class AcceptChildren
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Constant(1),
        Expression.Convert(Expression.Constant(1), typeof(short))
    ];

    private static readonly Expression[] ValidExpressions =
    [
        Expression.Add(Expression.Constant(1), Expression.Constant(1)),
        Expression.Subtract(Expression.Constant(1), Expression.Constant(1)),
        Expression.Subtract(Expression.Multiply(Expression.Constant(1), Expression.Constant(1)), Expression.Constant(1))
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.OfType<BinaryExpression>().HavingChild(node.AcceptChildren)),
        ExpressionTreeEvaluator.Create(node => node.OfType<BinaryExpression>().AcceptChildren())
    ];

    [Test, Combinatorial]
    public void ThrowsOnInvalidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(InvalidExpressions))]
        Expression invalidExpression)
    {
        Assert.That(() => evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }


    [Test, Combinatorial]
    public void DoesNotThrowOnValidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(ValidExpressions))]
        Expression validExpression)
    {
        Assert.That(() => evaluator.Evaluate(validExpression), Throws.Nothing);
    }
}
