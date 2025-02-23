using System.Linq.Expressions;

namespace TreeVal.Tests.Constant;

[TestFixture]
public class ByType
{
    private static readonly Type TChild = typeof(Child);
    private static readonly Type TBase = typeof(Base);
    
    private static readonly Child C1 = new();
    private static readonly Other O1 = new();

    private static readonly Expression InvalidExpression = Expression.Constant(O1);
    private static readonly Expression ValidExpression = Expression.Constant(C1);

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.Constant(TChild)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild))),

        ExpressionTreeEvaluator.Create(node => node.Constant<Child>()),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>())),

        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>()))
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
    }

    private class Base
    {
    }
}
