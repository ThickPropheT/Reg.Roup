using System.Linq.Expressions;
using Reg.Roup.Expression;

namespace Reg.Roup.Tests.Visitor
{
    [TestFixture]
    public class WithChildrenExpectation
    {
        private IBaseExpectation _expectation;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _expectation = ExpectNode
                .OfType(ExpressionType.Negate)
                .WithChildren((_, options) =>
                    options.NodeType(ExpressionType.Constant)
                );
        }

        [Test]
        public void ThrowsOnInvalidSchemas()
        {
            var invalidExpression = System.Linq.Expressions.Expression.Add(System.Linq.Expressions.Expression.Constant(1), System.Linq.Expressions.Expression.Constant(1));

            Assert.That(() => new VisitorEngine(_expectation).Visit(invalidExpression), Throws.Exception);
        }

        [Test]
        public void PassesThroughValidSchemas()
        {
            var validExpression = System.Linq.Expressions.Expression.Negate(System.Linq.Expressions.Expression.Constant(69));

            var validatedExpression = new VisitorEngine(_expectation).Visit(validExpression);

            Assert.That(validatedExpression, Is.Not.Null);

            var expressionResult = System.Linq.Expressions.Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

            Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Negate));
            Assert.That(expressionResult, Is.EqualTo(-69));
        }
    }
}
