using System.Linq.Expressions;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

[TestFixture]
public class Any
{
    private static readonly Expression[] ValidExpressions =
    [
        ExpressionTree.FromBody(() => DummyMethod.Static.GetString()),
        ExpressionTree.FromBody(() => new DummyMethod().GetString()),
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString()),

        ExpressionTree.FromBody(() => DummyMethod.Static.ReturnArg("1")),
        ExpressionTree.FromBody(() => new DummyMethod().ReturnArg("1")),
        ExpressionTree.FromBody(() => DummyMethod.Instance.ReturnArg("1")),

        ExpressionTree.FromBody(() => DummyMethod.Static.ReturnArg(DummyMethod.Static.GetString())),
        ExpressionTree.FromBody(() => new DummyMethod().ReturnArg(DummyMethod.Static.GetString())),
        ExpressionTree.FromBody(() => DummyMethod.Instance.ReturnArg(DummyMethod.Static.GetString())),

        ExpressionTree.FromBody(() =>
            DummyMethod.Static.ReturnArg(DummyMethod.Instance == new DummyMethod() ? "1" : "2")),
        ExpressionTree.FromBody(() =>
            new DummyMethod().ReturnArg(DummyMethod.Instance == new DummyMethod() ? "1" : "2")),
        ExpressionTree.FromBody(() =>
            DummyMethod.Instance.ReturnArg(DummyMethod.Instance == new DummyMethod() ? "1" : "2")),
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        ExpressionTree.FromBody(() => 1),
        ExpressionTree.FromBody(() => DummyProperty.Instance),
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        ExpressionTree.FromBody(() => new DummyProperty()),
        ExpressionTree.FromBody(() => new DummyProperty().String),
        ExpressionTree.FromBody<Func<string>>(() => DummyMethod.Instance.GetString),
        ExpressionTree.FromBody<Func<string>>(() => new DummyMethod().GetString),
    ];

    private static readonly ExpressionTreeEvaluator Evaluator =
        ExpressionTreeEvaluator.Create(node => node.MethodCall());

    [Test]
    public void DoesNotThrowOnValidSchemas([ValueSource(nameof(ValidExpressions))] Expression validExpression)
    {
        Assert.That(() => Evaluator.Evaluate(validExpression), Throws.Nothing);
    }

    [Test]
    public void ThrowsOnValidSchemas([ValueSource(nameof(InvalidExpressions))] Expression invalidExpression)
    {
        Assert.That(() => Evaluator.Evaluate(invalidExpression), Throws.TypeOf<TreeRejectedException>());
    }
}
