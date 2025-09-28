using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Expr.Constant;
using TreeVal.Primitives;

namespace TreeVal.Tests.Constant;

[TestFixtureSource(nameof(InvalidExpressions))]
public class Null
{
    private static readonly Type TChild = typeof(Child);
    private static readonly Type TBase = typeof(Base);

    private static readonly Expression[] InvalidExpressions =
    [
        Expression.Constant(new Child()),
        Expression.Constant(new Other())
    ];

    private readonly Expression _invalidExpression;
    private static readonly Expression ValidExpression = Expression.Constant(null, TChild);

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [ 
        // @formatter:off
        ExpressionTreeEvaluator.Create(node => node.Constant((Child?) null)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.Null())),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.EqualTo<object>(null))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.ReferenceEqualTo<object>(null))),

        ExpressionTreeEvaluator.Create(node => node.Constant(TChild, (Child?) null)),
        ExpressionTreeEvaluator.Create(node => node.Constant(TChild, EValue.Null())),
        ExpressionTreeEvaluator.Create(node => node.Constant(TChild, EValue.EqualTo<Child>(null))),
        ExpressionTreeEvaluator.Create(node => node.Constant(TChild, EValue.ReferenceEqualTo<Child>(null))),
        
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild), (Child?) null)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild), EValue.Null())),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild), EValue.EqualTo<Child>(null))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo(TChild), EValue.ReferenceEqualTo<Child>(null))),
        
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>(), (Child?) null)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>(), EValue.Null())),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>(), EValue.EqualTo<Child>(null))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.EqualTo<Child>(), EValue.ReferenceEqualTo<Child>(null))),
        
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase), (Child?) null)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase), EValue.Null())),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase), EValue.EqualTo<Child>(null))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is(TBase), EValue.ReferenceEqualTo<Child>(null))),
        
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>(), (Child?) null)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>(), EValue.Null())),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>(), EValue.EqualTo<Child>(null))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EType.Is<Base>(), EValue.ReferenceEqualTo<Child>(null))),
        
        ExpressionTreeEvaluator.Create(node => node.Constant<Child>(null)),
        ExpressionTreeEvaluator.Create(node => node.Constant<Base>(null)),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.Null<Child>())),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.Null<Base>())),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.EqualTo<Child>(null))),
        ExpressionTreeEvaluator.Create(node => node.Constant(EValue.ReferenceEqualTo<Child>(null)))
        // @formatter:on
    ];

    public Null(Expression invalidExpression)
    {
        _invalidExpression = invalidExpression;
    }

    [TestCaseSource(nameof(Evaluators))]
    public void ThrowsOnInvalidSchemas(ExpressionTreeEvaluator evaluator)
    {
        Assert.That(() => evaluator.Evaluate(_invalidExpression), Throws.TypeOf<TreeRejectedException>());
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
