using Reg.Roup.Expression;
using Reg.Roup.Tests.Resources;

namespace Reg.Roup.Tests.Visitor
{
    using System.Linq.Expressions;

    [TestFixture]
    public class OneOfExpectation
    {
        private static readonly Expression[] invalidExpressions = [
            Expression.Multiply(Expression.Constant(1), Expression.Constant(1)),
            Expression.Divide(Expression.Constant(1), Expression.Constant(1))
        ];

        private static readonly Expression[] validExpressions = [
            Expression.Add(Expression.Constant(42), Expression.Constant(27)),
            Expression.Subtract(Expression.Constant(420), Expression.Constant(351))
        ];

        private IBaseExpectation expectation;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectation = new ExpectationProxy((_, expectNode) =>
                    expectNode.OneOf(
                        expectNode.NodeType(ExpressionType.Add),
                        expectNode.NodeType(ExpressionType.Subtract)
                    )
            );
        }

        [TestCaseSource(nameof(invalidExpressions))]
        public void ThrowsOnInvalidSchemas(Expression invalidExpression)
        {
            Assert.That(() => new VisitorEngine(expectation).Visit(invalidExpression), Throws.Exception);
        }

        [TestCaseSource(nameof(validExpressions))]
        public void PassesThroughValidSchemas(Expression validExpression)
        {
            var validatedExpression = new VisitorEngine(expectation).Visit(validExpression);

            Assert.That(validatedExpression, Is.Not.Null);

            var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

            Assert.That(validatedExpression.NodeType, Is.EqualTo(validExpression.NodeType));
            Assert.That(expressionResult, Is.EqualTo(69));
        }
    }
}
