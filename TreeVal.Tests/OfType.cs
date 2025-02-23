using System.Linq.Expressions;

namespace TreeVal.Tests;

[TestFixture]
public class OfType
{
    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Break(Expression.Label()),
        Expression.Constant(1),
        Expression.New(typeof(object).GetConstructor(Type.EmptyTypes)!),
        Expression.Parameter(typeof(int), "index"),
        Expression.Rethrow(),
        Expression.DebugInfo(Expression.SymbolDocument("my.file"), 1, 1, 2, 2),
        Expression.Add(Expression.Constant(1), Expression.Constant(1)) 
    ];
    
    private static readonly Expression ValidExpression = Expression.Default(typeof(int));
    
    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node => node.OfType<DefaultExpression>());
    
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
