using System.Linq.Expressions;
using TreeVal.Diagnosticts;
using TreeVal.Eval;
using TreeVal.Expr;
using TreeVal.Expr.Conversion;
using TreeVal.Extensions;

namespace TreeVal.Tests.Cast;

[TestFixture]
public class Any
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Add(Expression.Constant(1), Expression.Constant(1))
    ];

    private static readonly Expression[] ValidExpressions =
    [
        Expression.Convert(Expression.Constant(1), typeof(short)),
        Expression.Convert(Expression.Add(Expression.Constant(1), Expression.Constant(1)), typeof(short)),
    ];

    private static readonly ExpressionTreeEvaluator Evaluator = ExpressionTreeEvaluator.Create(node => node.Cast());

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
