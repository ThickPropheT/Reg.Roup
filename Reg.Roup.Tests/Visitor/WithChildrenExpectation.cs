namespace Reg.Roup.Tests.Visitor
{
    using Reg.Roup.Expression;
    using System.Linq.Expressions;

    [TestFixture]
    public class WithChildrenExpectation
    {
        private IBaseExpectation expectation;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectation = _ExpectExt
                .NodeType(ExpressionType.Negate)
                .WithChildren((expression, options) =>
                    options.NodeType(ExpressionType.Constant)
                );
        }

        [Test]
        public void ThrowsOnInvalidSchemas()
        {
            var invalidExpression = Expression.Add(Expression.Constant(1), Expression.Constant(1));

            Assert.That(() => new VisitorEngine(expectation).Visit(invalidExpression), Throws.Exception);
        }

        [Test]
        public void PassesThroughValidSchemas()
        {
            var validExpression = Expression.Negate(Expression.Constant(69));

            var validatedExpression = new VisitorEngine(expectation).Visit(validExpression);

            Assert.That(validatedExpression, Is.Not.Null);

            var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

            Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Negate));
            Assert.That(expressionResult, Is.EqualTo(-69));
        }
    }
}
