using Reg.Roup.Expression;

namespace Reg.Roup.Tests.Visitor;

using System.Linq.Expressions;

[TestFixture]
public class EachExpectation
{
    private static readonly Expression[] InvalidExpressions = [
        Expression.Add(Expression.Constant(1), Expression.Add(Expression.Constant(1), Expression.Constant(2))),
        Expression.Subtract(Expression.Constant(2), Expression.Constant(4))
    ];
    
    private IBaseExpectation _expectation;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _expectation = _ExpectExt
            .NodeType<BinaryExpression>()
            .WithChildren((b, options) =>
                {
                    var children = new[] {b.Left, b.Right};
                    var values = new[] {1, 3};
                    // TODO does this obsolete 'Using'?
                    using var enumerator = (children as IEnumerable<Expression>).GetEnumerator();
                    
                    return options.Each(
                        enumerator,
                        _ => options
                            .NodeType<ConstantExpression>()
                            .Where(@const => @const.Value is int i && i == values[Array.IndexOf(children, @const)])
                    );
                }
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
        var subtractionExpression = Expression.Add(Expression.Constant(1), Expression.Constant(3));

        var validatedExpression = new VisitorEngine(_expectation).Visit(subtractionExpression);

        Assert.That(validatedExpression, Is.Not.Null);

        var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

        Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Add));
        Assert.That(expressionResult, Is.EqualTo(4));
    }
}
