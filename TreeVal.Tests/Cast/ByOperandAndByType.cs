using System.Linq.Expressions;
using TreeVal.Diagnosticts;
using TreeVal.Eval;
using TreeVal.Expr;
using TreeVal.Expr.Conversion;
using TreeVal.Extensions;

namespace TreeVal.Tests.Cast;

[TestFixture]
public class ByOperandAndByType
{
    private static readonly Expression InvalidExpression =
        Expression.Add(Expression.Constant(1), Expression.Constant(1));

    private static readonly Expression ValidExpression =
        Expression.Convert(Expression.Constant(1), typeof(short));

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.Cast<short>(node.Constant(1))),
        ExpressionTreeEvaluator.Create(node => node.Cast(node.Constant(1), typeof(short)))
    ];
    
    [TestCaseSource(nameof(Evaluators))]
    public void ThrowsOnInvalidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(InvalidExpression), Throws.TypeOf<TreeRejectedException>());
    }

    [TestCaseSource(nameof(Evaluators))]
    public void DoesNotThrowOnValidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(ValidExpression), Throws.Nothing);
    }
}
