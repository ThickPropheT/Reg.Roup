using System.Linq.Expressions;

namespace Treeval.Tests;

[TestFixture]
public class Constant
{
    private ExpressionVisitorNodeFactory _evaluator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _evaluator = ExpressionVisitorNodeFactory.Create(node => node.Constant(69));
    }
    
    [Test]
    public void ThrowsOnInvalidSchemas()
    {
        var invalidExpression = Expression.Add(Expression.Constant(1), Expression.Constant(1));
        
        Assert.That(() => _evaluator.Evaluate(invalidExpression), Throws.Exception);
    }
    
    [Test]
    public void DoesNotThrowOnValidSchemas()
    {
        var invalidExpression = Expression.Constant(69);
        
        Assert.That(() => _evaluator.Evaluate(invalidExpression),  Throws.Nothing);
    }
}
