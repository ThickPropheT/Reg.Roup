using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

[TestFixture]
public class ByDeclaringTypeAndByName
{
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionTree.FromBody(() => new DummyMethod().GetString1()),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1()),

        ExpressionTree.FromBody(() => new DummyMethod().GetString1("1")),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1("1")),

        ExpressionTree.FromBody(() => new DummyMethod().GetString1(DummyMethod.GetString2())),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1(DummyMethod.GetString2())),

        ExpressionTree.FromBody(() =>
            new DummyMethod().GetString1(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),

        ExpressionTree.FromBody(() =>
            DummyMethod.Instance.GetString1(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),
    ];

    private static readonly Expression[] ValidStaticExpressions =
    [
        ExpressionTree.FromBody(() => DummyMethod.GetString2()),
        ExpressionTree.FromBody(() => DummyMethod.GetString2("1")),
        ExpressionTree.FromBody(() => DummyMethod.GetString2(DummyMethod.GetString2())),
        ExpressionTree.FromBody(() =>
            DummyMethod.GetString2(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        ExpressionTree.FromBody(() => 1),
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1()),
        ExpressionTree.FromBody(() => new DummyMethod().GetString3()),
    ];

    private static readonly Expression[] InvalidStaticExpressions =
    [
        ExpressionTree.FromBody(() => DummyStaticMethod.GetString1()),
        ExpressionTree.FromBody(() => DummyMethod.GetString2()),
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.MethodCall<DummyMethod>(nameof(DummyMethod.GetString1))),
        ExpressionTreeEvaluator.Create(node => node.MethodCall(typeof(DummyMethod), nameof(DummyMethod.GetString1))),
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
    public void DoesNotThrowOnValidStaticMethodSchemas(
        [ValueSource(nameof(ValidStaticExpressions))]
        Expression validStaticMethodExpression)
    {
        var evaluator = ExpressionTreeEvaluator.Create(node =>
            node.MethodCall(typeof(DummyMethod), nameof(DummyMethod.GetString2)));

        Assert.That(() => evaluator.Evaluate(validStaticMethodExpression), Throws.Nothing);
    }

    [Test]
    public void DoesNotThrowOnValidExtensionMethodSchema()
    {
        var validExtensionMethodExpression = ExpressionTree.FromBody(() => new object().GetString3("0"));

        var evaluator = ExpressionTreeEvaluator.Create(node =>
            node.MethodCall(typeof(DummyExtensionMethod), nameof(DummyExtensionMethod.GetString3)));

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
    public void ThrowsOnInvalidStaticMethodSchemas(
        [ValueSource(nameof(InvalidStaticExpressions))]
        Expression invalidStaticMethodExpression)
    {
        var evaluator = ExpressionTreeEvaluator.Create(node =>
            node.MethodCall(typeof(DummyStaticMethod), nameof(DummyStaticMethod.GetString2)));

        Assert.That(() => evaluator.Evaluate(invalidStaticMethodExpression), Throws.TypeOf<TreeRejectedException>());
    }
}
