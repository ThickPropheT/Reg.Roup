using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Eval.AcceptChildren;
using TreeVal.Expr.Delegate;
using TreeVal.Tests.__Resources.Dummies;
using TreeVal.Tests.__Resources.Tools;

namespace TreeVal.Tests.MethodCallDelegate;

[TestFixture]
public class ByDelegateTypeAndWherePredicate
{
    private static readonly ExpressionData[] ValidExpressions =
    [
        ExpressionData.FromBody<Func<string, string>>(() => new object().GetString3)
            .WithName("extension"),

        ExpressionData.FromBody<Func<string, string>>(() => DummyInstanceMethod.Instance.GetString1)
            .WithName("instance"),

        ExpressionData.FromBody<Func<string, string>>(() => DummyMethod.GetString2)
            .WithName("static"),
    ];

    private static readonly Expression[] InvalidExpressions =
    [
        // TODO
        //  negative testing is probably pretty important here,
        //  but boy do i not feel like thinking that hard about it rn
        ExpressionData.FromBody<Func<string, string>>(() => DummyMethod.ReturnString)
            .WithName("static"),
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node =>
                node.MethodCallDelegate<Func<string, string>>(
                    where: method => method.Name.StartsWith("GetString")
                )
            )
            .WithName("default"),

        ExpressionTreeEvaluator.Create(node =>
                node.MethodCallDelegate<Func<string, string>>(
                    del => node.AcceptChildren(del),
                    method => method.Name.StartsWith("GetString")
                )
            )
            .WithName("default-like"),
    ];

    [Test, Combinatorial]
    public void DoesNotThrowOnValidSchemas(
        [ValueSource(nameof(Evaluators))] ExpressionTreeEvaluator evaluator,
        [ValueSource(nameof(ValidExpressions))]
        ExpressionData validExpression
    )
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
