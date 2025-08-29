using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

public class ByParameters
{
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionTree.FromBody(() => DummyMethod.GetString2("1")),
        ExpressionTree.FromBody(() => new DummyMethod().GetString1("2")),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1("3")),
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1("4")),

        ExpressionTree.FromBody(() => DummyMethod.GetString2(DummyMethod.GetString2())),
        ExpressionTree.FromBody(() => new DummyMethod().GetString1(DummyMethod.GetString2())),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1(DummyMethod.GetString2())),

        ExpressionTree.FromBody(() =>
            DummyMethod.GetString2(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),

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

    private static readonly Expression[] InvalidExpressions =
    [
        // TODO
        // ExpressionTree.FromBody(() => 1),
        // ExpressionTree.FromBody(() => DummyProperty.Instance),
        // ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        // ExpressionTree.FromBody(() => new DummyProperty()),
        // ExpressionTree.FromBody(() => new DummyProperty().String),
        // ExpressionTree.FromBody<Func<string>>(() => DummyMethod.Instance.GetString1),
        // ExpressionTree.FromBody<Func<string>>(() => new DummyMethod().GetString1),
        // ExpressionTree.FromBody(() => DummyStaticMethod.GetString2()),
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.MethodCall(
                node.AnyOne()
                    .Equals(e => e.Type, typeof(string))
                    .HavingAnyChild()
            ))
            .WithName("params"),

        ExpressionTreeEvaluator.Create(node => node.MethodCall([]))
            .WithName("empty params"),

        ExpressionTreeEvaluator.Create(node => node.MethodCall(_ =>
            [
                node.AnyOne()
                    .Equals(e => e.Type, typeof(string))
                    .HavingAnyChild()
            ]))
            .WithName("lambda"),

        ExpressionTreeEvaluator.Create(node => node.MethodCall(_ => []))
            .WithName("empty lambda"),
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
