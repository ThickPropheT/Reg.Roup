using System.Linq.Expressions;
using TreeVal.Extensions;

namespace TreeVal.Tests.Constant;

[TestFixture]
public class ByTypeAndValue
{
    private static readonly Type TChild = typeof(Child);
    private static readonly Type TBase = typeof(Base);

    private static readonly Child C1 = new();
    private static readonly Child C2 = new();
    private static readonly Other O1 = new();

    private static readonly Expression InvalidExpression = Expression.Constant(O1);
    private static readonly Expression ValidExpression = Expression.Constant(C1);

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.Constant(TChild, C1)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild), C1)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>(), C1)),
        ExpressionTreeEvaluator.Create(node => node.Constant<Child>(C1)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase), C1)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>(), C1)),
        ExpressionTreeEvaluator.Create(node => node.Constant<Base>(C1)),

        ExpressionTreeEvaluator.Create(node => node.Constant(TChild, EValue.EqualTo(C2))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild), EValue.EqualTo(C2))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>(), EValue.EqualTo(C2))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.EqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase), EValue.EqualTo(C2))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>(), EValue.EqualTo(C2))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.EqualTo<Base>(C1))),

        ExpressionTreeEvaluator.Create(node => node.Constant(TChild, EValue.ReferenceEqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild), EValue.ReferenceEqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>(), EValue.ReferenceEqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.ReferenceEqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase), EValue.ReferenceEqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>(), EValue.ReferenceEqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.ReferenceEqualTo(C1))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.ReferenceEqualTo<Base>(C1))),
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
