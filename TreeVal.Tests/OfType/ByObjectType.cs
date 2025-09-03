using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Eval;
using TreeVal.Expr;
using TreeVal.Extensions;

namespace TreeVal.Tests.OfType;

[TestFixture]
public class ByObjectType
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Break(Expression.Label()),
        Expression.Constant(1),
        Expression.New(typeof(object).GetConstructor(Type.EmptyTypes)!),
        Expression.Parameter(typeof(int), "index"),
        Expression.Rethrow(),
        Expression.DebugInfo(Expression.SymbolDocument("my.file"), 1, 1, 2, 2),
        Expression.Add(Expression.Constant(1), Expression.Constant(1)),
    ];

    private static readonly Expression[] ValidExpressions =
    [
        Expression.Convert(Expression.Constant(1), typeof(short)),
        Expression.Unbox(Expression.Constant(new object()), typeof(short)),
        Expression.Throw(Expression.Constant(new Exception()))
    ];

    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node => node.OfType<UnaryExpression>().HavingChildren(node.AnyOne()));

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
