using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

[TestFixture]
public class Any
{
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionTree.FromBody(() => DummyStaticMethod.GetString1()),
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1()),
        ExpressionTree.FromBody(() => DummyInstanceMethod.Instance.GetString1()),

        ExpressionTree.FromBody(() => DummyStaticMethod.GetString1("1")),
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1("1")),
        ExpressionTree.FromBody(() => DummyInstanceMethod.Instance.GetString1("1")),

        ExpressionTree.FromBody(() => DummyStaticMethod.GetString1(DummyStaticMethod.GetString1())),
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1(DummyStaticMethod.GetString1())),
        ExpressionTree.FromBody(() => DummyInstanceMethod.Instance.GetString1(DummyStaticMethod.GetString1())),

        ExpressionTree.FromBody(() =>
            DummyStaticMethod.GetString1(
                DummyInstanceMethod.Instance == new DummyInstanceMethod()
                    ? "1"
                    : "2")),

        ExpressionTree.FromBody(() =>
            new DummyInstanceMethod().GetString1(
                DummyInstanceMethod.Instance == new DummyInstanceMethod()
                    ? "1"
                    : "2")),

        ExpressionTree.FromBody(() =>
            DummyInstanceMethod.Instance.GetString1(
                DummyInstanceMethod.Instance == new DummyInstanceMethod()
                    ? "1"
                    : "2")),
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        ExpressionTree.FromBody(() => 1),
        ExpressionTree.FromBody(() => DummyProperty.Instance),
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        ExpressionTree.FromBody(() => new DummyProperty()),
        ExpressionTree.FromBody(() => new DummyProperty().String),
        ExpressionTree.FromBody<Func<string>>(() => DummyInstanceMethod.Instance.GetString1),
        ExpressionTree.FromBody<Func<string>>(() => new DummyInstanceMethod().GetString1),
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.MethodCall())
    ];

    [Test]
    public void DoesNotThrowOnValidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(ValidExpressions))]
        Expression validExpression)
    {
        Assert.That(() => evaluator.Evaluate(validExpression), Throws.Nothing);
    }

    [Test]
    public void ThrowsOnInvalidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(InvalidExpressions))]
        Expression invalidExpression)
    {
        Assert.That(() => evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }
}
