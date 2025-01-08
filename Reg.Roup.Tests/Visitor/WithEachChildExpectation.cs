using Reg.Roup.Expression;

namespace Reg.Roup.Tests.Visitor;

using System.Linq.Expressions;

[TestFixture]
public class WithEachChildExpectation
{
    private static readonly Expression[] InvalidExpressions = [
        Expression.Add(Expression.Constant(0), Expression.Add(Expression.Constant(1), Expression.Constant(2))),
        Expression.Subtract(Expression.Constant(1), Expression.Constant(2)),
        Expression.Add(Expression.Constant(1), Expression.Constant(1))
    ];
    
    private IBaseExpectation _expectation;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _expectation = ExpectNode
            .OfType<BinaryExpression>()
            .WithEachChild(
                bin => new[] {bin.Left, bin.Right},
                (bin, child, i, options) => // TODO is there a good way to verify 'child'?
                    options
                        .NodeType<ConstantExpression>()
                        .Where(@const => @const.Value is int v && v == i)
            );
    }
    
    [TestCaseSource(nameof(InvalidExpressions))]
    public void ThrowsOnInvalidSchemas(Expression invalidExpression)
    {
        Assert.That(() => new VisitorEngine(_expectation).Visit(invalidExpression), Throws.Exception);
    }
    
    [Test]
    public void PassesThroughValidSchemas()
    {
        var expression = Expression.Add(Expression.Constant(0), Expression.Constant(1));

        var validatedExpression = new VisitorEngine(_expectation).Visit(expression);

        Assert.That(validatedExpression, Is.Not.Null);

        var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

        Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Add));
        Assert.That(expressionResult, Is.EqualTo(1));
    }
}
