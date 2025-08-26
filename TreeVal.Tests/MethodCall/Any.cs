using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

[TestFixture]
public class Any
{
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionTree.FromBody(() => DummyMethod2.GetString1()),
        ExpressionTree.FromBody(() => new DummyMethod1().GetString1()),
        ExpressionTree.FromBody(() => DummyMethod1.Instance.GetString1()),

        ExpressionTree.FromBody(() => DummyMethod2.GetString1("1")),
        ExpressionTree.FromBody(() => new DummyMethod1().GetString1("1")),
        ExpressionTree.FromBody(() => DummyMethod1.Instance.GetString1("1")),

        ExpressionTree.FromBody(() => DummyMethod2.GetString1(DummyMethod2.GetString1())),
        ExpressionTree.FromBody(() => new DummyMethod1().GetString1(DummyMethod2.GetString1())),
        ExpressionTree.FromBody(() => DummyMethod1.Instance.GetString1(DummyMethod2.GetString1())),

        ExpressionTree.FromBody(() =>
            DummyMethod2.GetString1(DummyMethod1.Instance == new DummyMethod1() ? "1" : "2")),
        ExpressionTree.FromBody(() =>
            new DummyMethod1().GetString1(DummyMethod1.Instance == new DummyMethod1() ? "1" : "2")),
        ExpressionTree.FromBody(() =>
            DummyMethod1.Instance.GetString1(DummyMethod1.Instance == new DummyMethod1() ? "1" : "2")),
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        ExpressionTree.FromBody(() => 1),
        ExpressionTree.FromBody(() => DummyProperty.Instance),
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        ExpressionTree.FromBody(() => new DummyProperty()),
        ExpressionTree.FromBody(() => new DummyProperty().String),
        ExpressionTree.FromBody<Func<string>>(() => DummyMethod1.Instance.GetString1),
        ExpressionTree.FromBody<Func<string>>(() => new DummyMethod1().GetString1),
    ];

    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node => node.MethodCall());

    [Test]
    public void DoesNotThrowOnValidSchemas([ValueSource(nameof(ValidExpressions))] Expression validExpression)
    {
        Assert.That(() => Evaluator.Evaluate(validExpression), Throws.Nothing);
    }

    [Test]
    public void ThrowsOnInvalidSchemas([ValueSource(nameof(InvalidExpressions))] Expression invalidExpression)
    {
        Assert.That(() => Evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }
}
