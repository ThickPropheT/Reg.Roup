using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

// accept: method calls to methods named "GetString1" with target: instance via ctor
[TestFixture]
public class ByNameAndTarget
{
    private static readonly Expression[] ValidExpressions =
    [
        // accept: target: instance via ctor, arg[0]: n/a
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1()),
        // accept: target: instance via ctor, arg[0]: constant
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1("1")),
        // accept: target: instance via ctor, arg[0]: result of method call
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1(DummyStaticMethod.GetString1())),
        // accept: extension method target: instance via ctor, arg[0]: constant
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1("2", null)),
        
        // accept: target: instance of any type via ctor, arg[0]: n/a
        ExpressionTree.FromBody(() => new DummyMethod().GetString1()),
        // accept: extension method target: instance of any type via ctor, arg[0]: result of method call
        ExpressionTree.FromBody(() => new object().GetString1(DummyMethod.GetString2(), null)),
        
        // accept: target: instance via ctor, arg[0]: result of ternary
        ExpressionTree.FromBody(() =>
            new DummyInstanceMethod().GetString1(
                DummyInstanceMethod.Instance == new DummyInstanceMethod()
                    ? "1"
                    : "2")),
        
        // accept: extension method target: instance via ctor, arg[0]: result of ternary
        ExpressionTree.FromBody(() =>
            new DummyInstanceMethod().GetString1(
                DummyInstanceMethod.Instance == new DummyInstanceMethod()
                    ? "1"
                    : "2",
                null)),
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        // reject: severely wrong type
        ExpressionTree.FromBody(() => 1),
        ExpressionTree.FromBody(() => DummyProperty.Instance),
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        ExpressionTree.FromBody(() => new DummyProperty()),
        ExpressionTree.FromBody(() => new DummyProperty().String),
        ExpressionTree.FromBody<Func<string>>(() => DummyInstanceMethod.Instance.GetString1),
        ExpressionTree.FromBody<Func<string>>(() => new DummyInstanceMethod().GetString1),
        // reject: target: static, arg[0]: n/a
        ExpressionTree.FromBody(() => DummyStaticMethod.GetString1()),
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.MethodCall(nameof(DummyMethod.GetString1), _ => node.New())),
    ];

    [Test, Combinatorial]
    public void DoesNotThrowOnValidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(ValidExpressions))]
        Expression validExpression)
    {
        Assert.That(() => evaluator.Evaluate(validExpression), Throws.Nothing);
    }

    [Test, Combinatorial]
    public void ThrowsOnInvalidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(InvalidExpressions))]
        Expression invalidExpression)
    {
        Assert.That(() => evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }
}
