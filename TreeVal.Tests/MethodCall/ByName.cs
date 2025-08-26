using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

[TestFixture]
public class ByName
{
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionTree.FromBody(() => DummyMethod.Static.GetString1()),
        ExpressionTree.FromBody(() => new DummyMethod().GetString1()),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1()),

        ExpressionTree.FromBody(() => DummyMethod.Static.GetString1("1")),
        ExpressionTree.FromBody(() => new DummyMethod().GetString1("1")),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1("1")),

        ExpressionTree.FromBody(() => DummyMethod.Static.GetString1(DummyMethod.Static.GetString1())),
        ExpressionTree.FromBody(() => new DummyMethod().GetString1(DummyMethod.Static.GetString1())),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1(DummyMethod.Static.GetString1())),

        ExpressionTree.FromBody(() =>
            DummyMethod.Static.GetString1(DummyMethod.Instance == new DummyMethod() ? "1" : "2")),
        ExpressionTree.FromBody(() =>
            new DummyMethod().GetString1(DummyMethod.Instance == new DummyMethod() ? "1" : "2")),
        ExpressionTree.FromBody(() =>
            DummyMethod.Instance.GetString1(DummyMethod.Instance == new DummyMethod() ? "1" : "2"))
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        ExpressionTree.FromBody(() => 1),
        ExpressionTree.FromBody(() => DummyProperty.Instance),
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        ExpressionTree.FromBody(() => new DummyProperty()),
        ExpressionTree.FromBody(() => new DummyProperty().String),
        ExpressionTree.FromBody<Func<string>>(() => DummyMethod.Instance.GetString1),
        ExpressionTree.FromBody<Func<string>>(() => new DummyMethod().GetString1),
        ExpressionTree.FromBody(() => DummyMethod.Static.GetString2()),
        ExpressionTree.FromBody(() => new DummyMethod().GetString2()),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString2())
    ];

    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node => node.MethodCall(nameof(DummyMethod.GetString1)));

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
