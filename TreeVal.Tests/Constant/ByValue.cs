using System.Linq.Expressions;
using TreeVal.Extensions;

namespace TreeVal.Tests.Constant;

[TestFixture]
public class ByValue
{
    private static readonly Child C1 = new();
    private static readonly Child C2 = new();
    private static readonly Other O1 = new();

    private static readonly Expression InvalidExpression = Expression.Constant(O1);
    private static readonly Expression ValidExpression = Expression.Constant(C1);

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.Constant(C1)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.EqualTo<object>(C2))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.ReferenceEqualTo<object>(C1)))
    ];

    [TestCaseSource(nameof(Evaluators))]
    public void ThrowsOnInvalidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(InvalidExpression), Throws.TypeOf<TreeRejectedException>());
    }

    [TestCaseSource(nameof(Evaluators))]
    public void DoesNotThrowOnValidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(ValidExpression), Throws.Nothing);
    }

    private class Other
    {
    }

    private class Child : Base
    {
        public override bool Equals(object? obj) => obj is Child;
        public override int GetHashCode() => 1;
    }

    private class Base
    {
    }
}
