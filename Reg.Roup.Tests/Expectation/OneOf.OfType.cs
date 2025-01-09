using System.Linq.Expressions;
using Reg.Roup.Expectation;
using Reg.Roup.Expectation.Common.OfType;

namespace Reg.Roup.Tests.Expectation;

[TestFixture]
public partial class OneOf
{
    [TestFixture]
    public class OfType
    {
        private static readonly Expression[] InvalidExpressions =
        [
            Expression.Multiply(Expression.Constant(1), Expression.Constant(1)),
            Expression.Divide(Expression.Constant(1), Expression.Constant(1))
        ];

        private static readonly Expression[] ValidExpressions =
        [
            Expression.Add(Expression.Constant(42), Expression.Constant(27)),
            Expression.Subtract(Expression.Constant(420), Expression.Constant(351))
        ];

        private IBaseExpectation _expectation;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _expectation = ExpectNode
                .OneOf(expectNode =>
                [
                    expectNode.OfType(ExpressionType.Add),
                    expectNode.OfType(ExpressionType.Subtract)
                ]);
        }

        [TestCaseSource(nameof(InvalidExpressions))]
        public void ThrowsOnInvalidSchemas(Expression invalidExpression)
        {
            Assert.That(() => new VisitorEngine(_expectation).Visit(invalidExpression), Throws.Exception);
        }

        [TestCaseSource(nameof(ValidExpressions))]
        public void PassesThroughValidSchemas(Expression validExpression)
        {
            var validatedExpression = new VisitorEngine(_expectation).Visit(validExpression);

            Assert.That(validatedExpression, Is.Not.Null);

            var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

            Assert.That(validatedExpression.NodeType, Is.EqualTo(validExpression.NodeType));
            Assert.That(expressionResult, Is.EqualTo(69));
        }
    }
}
