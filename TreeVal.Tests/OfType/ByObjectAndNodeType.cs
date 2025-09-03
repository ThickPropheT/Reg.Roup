using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Eval;
using TreeVal.Expr;
using TreeVal.Extensions;

namespace TreeVal.Tests.OfType;

[TestFixture]
public class ByObjectAndNodeType
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Break(Expression.Label()),
        Expression.Constant(1),
        Expression.Convert(Expression.Constant(1), typeof(short)),
        Expression.New(typeof(object).GetConstructor(Type.EmptyTypes)!),
        Expression.Parameter(typeof(int), "index"),
        Expression.Rethrow(),
        Expression.DebugInfo(Expression.SymbolDocument("my.file"), 1, 1, 2, 2),
        Expression.Subtract(Expression.Constant(1), Expression.Constant(1)),
        Expression.Unbox(Expression.Constant(new object()), typeof(short))
    ];

    private static readonly Expression ValidExpression = Expression.Add(Expression.Constant(1), Expression.Constant(1));

    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node =>
            node
                .OfType<BinaryExpression>(ExpressionType.Add)
                .HavingChildren(
                    node.AnyOne(),
                    node.AnyOne()
                )
        );

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
