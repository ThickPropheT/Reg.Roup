using Reg.Roup.Expression;

namespace Reg.Roup.Tests.Visitor
{
    using System.Linq.Expressions;

    [TestFixture]
    public partial class NodeTypeExpectation
    {
        [Test]
        public void ThrowsOnInvalidSchemas()
        {
            var invalidSchema = Expression.Add(Expression.Constant(0), Expression.Constant(0));
            var validation = _ExpectExt.NodeType(ExpressionType.Subtract);

            Assert.That(() => new VisitorEngine(validation).Visit(invalidSchema), Throws.Exception);
        }

        [Test]
        public void PassThroughValidSchemas()
        {
            var validSchema = Expression.Subtract(Expression.Constant(69), Expression.Constant(27));
            var validation = _ExpectExt.NodeType(ExpressionType.Subtract);

            var validatedExpression = new VisitorEngine(validation).Visit(validSchema);

            Assert.That(validatedExpression, Is.Not.Null);

            var expressionResult = Expression.Lambda(validatedExpression).Compile().DynamicInvoke();

            Assert.That(validatedExpression.NodeType, Is.EqualTo(ExpressionType.Subtract));
            Assert.That(expressionResult, Is.EqualTo(42));
        }
    }
}
