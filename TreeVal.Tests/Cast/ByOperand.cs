using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Eval;
using TreeVal.Expr;
using TreeVal.Expr.Conversion;
using TreeVal.Extensions;

namespace TreeVal.Tests.Cast;

[TestFixture]
public class ByOperand
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Add(Expression.Constant(1), Expression.Constant(1)),
        Expression.Convert(Expression.Constant(2), typeof(short)),
        Expression.Unbox(Expression.Constant(new object()), typeof(short)),
        Expression.Throw(Expression.Constant(new Exception())),
        Expression.Rethrow()
    ];

    private static readonly Expression ValidExpression =
        Expression.Convert(Expression.Constant(1), typeof(short));

    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node => node.Cast(node.Constant(1)));

    [TestCaseSource(nameof(InvalidExpressions))]
    public void ThrowsOnInvalidSchemas(Expression invalidExpression)
    {
        Assert.That(() => Evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }

    [Test]
    public void DoesNotThrowOnValidSchemas()
    {
        Assert.That(() => Evaluator.Evaluate(ValidExpression), Throws.Nothing);
    }
}
