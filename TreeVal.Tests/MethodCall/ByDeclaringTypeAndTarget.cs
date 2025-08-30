using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

[TestFixture]
public class ByDeclaringTypeAndByTarget
{
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionTree.FromBody(() => new DummyMethod().GetString3()),
        ExpressionTree.FromBody(() => new DummyMethod().GetString1("1")),
        ExpressionTree.FromBody(() => new DummyMethod().GetString1(DummyMethod.GetString2())),

        ExpressionTree.FromBody(() =>
            new DummyMethod().GetString1(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        ExpressionTree.FromBody(() => 1),
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1()),
        // reject: target: instance via property access
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1()),
        // reject: target: static
        ExpressionTree.FromBody(() => DummyMethod.GetString2()),
        // reject: extension method not defined in DummyMethod
        ExpressionTree.FromBody(() => new DummyMethod().GetString1("1", null)),
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.MethodCall<DummyMethod>(_ => node.New())),
        ExpressionTreeEvaluator.Create(node => node.MethodCall(typeof(DummyMethod), _ => node.New())),
    ];

    [Test, Combinatorial]
    public void DoesNotThrowOnValidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(ValidExpressions))]
        Expression validExpression)
    {
        Assert.That(() => evaluator.Evaluate(validExpression), Throws.Nothing);
    }

    [Test]
    public void DoesNotThrowOnValidExtensionMethodSchema()
    {
        var validExtensionMethodExpression = ExpressionTree.FromBody(() => new object().GetString3("0"));

        var evaluator = ExpressionTreeEvaluator.Create(node =>
            node.MethodCall(typeof(DummyExtensionMethod), _ => node.New()));

        Assert.That(() => evaluator.Evaluate(validExtensionMethodExpression), Throws.Nothing);
    }

    [Test, Combinatorial]
    public void ThrowsOnInvalidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(InvalidExpressions))]
        Expression invalidExpression)
    {
        Assert.That(() => evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }
    
    [Test]
    public void ThrowsOnInvalidExtensionMethodSchema()
    {
        Assert.Inconclusive("Not Implemented");
    }
}
