using Reg.Roup.Expression;

namespace Reg.Roup.Tests.Visitor
{
    using System.Linq.Expressions;

    [TestFixture]
    public class NodeTypeExpectation
    {
        private IExpectation subtractionExpectation;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            subtractionExpectation = _ExpectExt.NodeType(ExpressionType.Subtract);
        }

        [Test]
        public void ThrowsOnInvalidSchemas()
        {
            var additionExpression = Expression.Add(Expression.Constant(0), Expression.Constant(0));

            Assert.That(() => new VisitorEngine(subtractionExpectation).Visit(additionExpression), Throws.Exception);
        }

        [Test]
        public void PassesThroughValidSchemas()
        {
            var subtractionExpression = Expression.Subtract(Expression.Constant(69), Expression.Constant(27));

            var validatedExpression = new VisitorEngine(subtractionExpectation).Visit(subtractionExpression);

            Assert.That(validatedExpression, Is.Not.Null);

            var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

            Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Subtract));
            Assert.That(expressionResult, Is.EqualTo(42));
        }
    }
}
