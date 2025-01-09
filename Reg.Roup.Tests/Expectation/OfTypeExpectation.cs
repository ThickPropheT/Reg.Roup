using System.Linq.Expressions;
using Reg.Roup.Expectation;

namespace Reg.Roup.Tests.Expectation;

[TestFixture]
public class OfTypeExpectation
{
    private IBaseExpectation _expectation;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _expectation = ExpectNode.OfType(ExpressionType.Subtract);
    }

    [Test]
    public void ThrowsOnInvalidSchemas()
    {
        var expression = Expression.Add(Expression.Constant(0), Expression.Constant(0));

        Assert.That(() => new VisitorEngine(_expectation).Visit(expression), Throws.Exception);
    }

    [Test]
    public void PassesThroughValidSchemas()
    {
        var expression = Expression.Subtract(Expression.Constant(69), Expression.Constant(27));

        var validatedExpression = new VisitorEngine(_expectation).Visit(expression);

        Assert.That(validatedExpression, Is.Not.Null);

        var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

        Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Subtract));
        Assert.That(expressionResult, Is.EqualTo(42));
    }
}
