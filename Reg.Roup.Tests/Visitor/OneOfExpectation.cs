using Reg.Roup.Expression;

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
            expectation = new ExpectationProxy(
                new Expect(),
                (_, options) =>
                    options.OneOf(
                        options.NodeType(ExpressionType.Add),
                        options.NodeType(ExpressionType.Subtract)
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

        // TODO consider making this class a real boy or at the very least a shared test resource
        private class ExpectationProxy : IBaseExpectation
        {
            private readonly IExpectationOptions options;
            private readonly IBaseExpectation.Next<Expression> seek;

            public IExpectationOptions Options => options;

            public ExpectationProxy(IExpectationOptions options, IBaseExpectation.Next<Expression> seek)
            {
                this.options = options;
                this.seek = seek;
            }

            public void AddCondition(IBaseExpectation.Condition<Expression> condition)
                => throw new NotSupportedException();

            public void SetNext(IBaseExpectation.Next<Expression> seek)
                => throw new NotSupportedException();

            public TExpectation TransferTo<TExpectation>(TExpectation expectation) where TExpectation : IBaseExpectation
            {
                expectation.SetNext(seek);
                return expectation;
            }

            public IEvaluationFrame BuildFrame(Expression? node) 
                => seek!.Invoke(node!, options).BuildFrame(node);
        }
    }
}
