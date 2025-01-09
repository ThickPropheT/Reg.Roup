using System.Linq.Expressions;
using Reg.Roup.Expectation;

namespace Reg.Roup.Tests.Expectation;

[TestFixture]
public class WithChildren
{
    private IBaseExpectation _expectation;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _expectation = ExpectNode
            .OfType(ExpressionType.Negate)
            .WithChildren((_, expectNode) =>
                expectNode.OfType(ExpressionType.Constant)
            );
    }

    [Test]
    public void ThrowsOnInvalidSchemas()
    {
        var invalidExpression = Expression.Add(Expression.Constant(1), Expression.Constant(1));

        Assert.That(() => new VisitorEngine(_expectation).Visit(invalidExpression), Throws.Exception);
    }

    [Test]
    public void PassesThroughValidSchemas()
    {
        var validExpression = Expression.Negate(Expression.Constant(69));

        var validatedExpression = new VisitorEngine(_expectation).Visit(validExpression);

        Assert.That(validatedExpression, Is.Not.Null);

        var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

        Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Negate));
        Assert.That(expressionResult, Is.EqualTo(-69));
    }
}
