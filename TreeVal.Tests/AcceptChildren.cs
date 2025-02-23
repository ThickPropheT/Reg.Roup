using System.Linq.Expressions;

namespace TreeVal.Tests;

[TestFixture]
public class AcceptChildren
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Break(Expression.Label()),
        Expression.Constant(1),
        Expression.Continue(Expression.Label()),
        Expression.Default(typeof(int)),
        // Expression.Dynamic(), // TODO
        Expression.Empty(),
        Expression.Goto(Expression.Label()),
        Expression.New(typeof(object).GetConstructor(Type.EmptyTypes)!),
        Expression.Parameter(typeof(int), "index"),
        Expression.Rethrow(),
        Expression.Return(Expression.Label()),
        Expression.Variable(typeof(int)),
        Expression.DebugInfo(Expression.SymbolDocument("my.file"), 1, 1, 2, 2),
        Expression.ClearDebugInfo(Expression.SymbolDocument("my.file")),
        Expression.Convert(Expression.Constant(1), typeof(short)),
    ];
    
    private static readonly Expression[] ValidExpressions =
    [
        Expression.Add(Expression.Constant(1), Expression.Constant(1)),
        Expression.Subtract(Expression.Constant(1), Expression.Constant(1)),
    ];

    

    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node =>
            node
                .OfType<BinaryExpression>()
                .HavingChildren(binary =>
                [
                    node.AcceptChildren(binary)
                ])
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
