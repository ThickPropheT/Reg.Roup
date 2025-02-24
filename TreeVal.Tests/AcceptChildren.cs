using System.Linq.Expressions;
using TreeVal.Extensions;

namespace TreeVal.Tests;

[TestFixtureSource(nameof(Evaluators))]
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

    private readonly ExpressionTreeEvaluator _evaluator;

    public AcceptChildren(ExpressionTreeEvaluator evaluator)
    {
        _evaluator = evaluator;
    }

    [TestCaseSource(nameof(InvalidExpressions))]
    public void ThrowsOnInvalidSchemas(Expression invalidExpression)
    {
        Assert.That(() => _evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }

    [TestCaseSource(nameof(ValidExpressions))]
    public void DoesNotThrowOnValidSchemas(Expression validExpression)
    {
        Assert.That(() => _evaluator.Evaluate(validExpression), Throws.Nothing);
    }
}
