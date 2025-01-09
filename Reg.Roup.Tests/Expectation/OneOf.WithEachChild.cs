using System.Linq.Expressions;
using Reg.Roup.Expectation;
using Reg.Roup.Expectation.Common.OfType;
using Reg.Roup.Expectation.Common.Where;
using Reg.Roup.Expectation.Common.WithEachChild;

namespace Reg.Roup.Tests.Expectation;

[TestFixture]
public partial class OneOf
{
    [TestFixture]
    public class WithEachChild
    {
        private static readonly Expression[] InvalidExpressions =
        [
            Expression.Add(Expression.Constant(0), Expression.Add(Expression.Constant(1), Expression.Constant(2))),
            Expression.Subtract(Expression.Constant(0), Expression.Constant(2)),
            Expression.Add(Expression.Constant(1), Expression.Constant(1))
        ];

        private static readonly Expression[] ValidExpressions =
        [
            Expression.Add(Expression.Constant(0), Expression.Constant(1)),
            Expression.Constant(1)
        ];
        
        private IBaseExpectation _expectation;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _expectation = ExpectNode
                .OneOf(expectNode =>
                [
                    expectNode
                        .OfType<BinaryExpression>()
                        .WithEachChild(
                            bin => new [] {bin.Left, bin.Right},
                            // TODO figure out what to do about doubled up 'expectNode's
                            (bin, child, i, expectNode) =>
                                expectNode
                                    .OfType<ConstantExpression>()
                                    .Where(@const => @const.Value is int v && v == i)
                        ),
                    expectNode.OfType(ExpressionType.Constant)
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
            Assert.That(expressionResult, Is.EqualTo(1));
        }
    }
}
