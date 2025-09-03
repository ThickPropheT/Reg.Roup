using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Eval;
using TreeVal.Eval.Object;
using TreeVal.Expr;
using TreeVal.Extensions;
using TreeVal.Tests.__Resources.Dummies;

namespace TreeVal.Tests.MethodCall;

// TODO it'd be cool to find a way to display the comments below in the test results
// accept: any method call with arg[0] resolving to string
[TestFixture]
public class ByParameters
{
    private static readonly Expression[] ValidExpressions =
    [
        // accept: target: static, arg[0]: constant
        ExpressionTree.FromBody(() => DummyMethod.GetString2("1")),
        // accept: target: instance via ctor, arg[0]: constant
        ExpressionTree.FromBody(() => new DummyMethod().GetString1("2")),
        // accept: target: instance via property access, arg[0]: constant
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1("3")),
        // accept: target: instance of any type via ctor, arg[0]: constant
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1("4")),
        // accept: extension method target: instance of any type via ctor, arg[0]: constant
        ExpressionTree.FromBody(() => new object().GetString3("5")),

        // accept: target: static, arg[0]: result of method call
        ExpressionTree.FromBody(() => DummyMethod.GetString2(DummyMethod.GetString2())),
        // accept: target: instance via ctor, arg[0]: result of method call
        ExpressionTree.FromBody(() => new DummyMethod().GetString1(DummyMethod.GetString2())),
        // accept: target: instance via property access, arg[0]: result of method call
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1(DummyMethod.GetString2())),
        // accept: target: instance of any type via ctor, arg[0]: result of method call
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1(DummyMethod.GetString2())),
        // accept: extension method target: instance of any type via ctor, arg[0]: result of method call
        ExpressionTree.FromBody(() => new object().GetString3(DummyMethod.GetString2())),

        // accept: target: static, arg[0]: result of ternary
        ExpressionTree.FromBody(() =>
            DummyMethod.GetString2(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),

        // accept: target: instance via ctor, arg[0]: result of ternary
        ExpressionTree.FromBody(() =>
            new DummyMethod().GetString1(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),

        // accept: target: instance via property access, arg[0]: result of ternary
        ExpressionTree.FromBody(() =>
            DummyMethod.Instance.GetString1(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),
        
        // accept: target: instance of any type via ctor, arg[0]: result of ternary
        ExpressionTree.FromBody(() =>
            new DummyInstanceMethod().GetString1(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),
        
        // accept: extension method target: instance of any type via ctor, arg[0]: result of ternary
        ExpressionTree.FromBody(() =>
            new object().GetString3(
                DummyMethod.Instance == new DummyMethod()
                    ? "1"
                    : "2")),
    ];
    
    private static readonly Expression[] InvalidExpressions =
    [
        // reject: severely wrong type
        ExpressionTree.FromBody(() => 1),
        // reject: method call delegate
        ExpressionTree.FromBody(() => DummyProperty.Instance.String),
        
        // reject: target: static, arg[0]: constant
        ExpressionTree.FromBody(() => DummyMethod.GetString2()),
        // reject: target: instance via ctor, arg[0]: constant
        ExpressionTree.FromBody(() => new DummyMethod().GetString1()),
        // reject: target: instance via property access, arg[0]: constant
        ExpressionTree.FromBody(() => DummyMethod.Instance.GetString1()),
        // reject: target: instance of any type via ctor, arg[0]: constant
        ExpressionTree.FromBody(() => new DummyInstanceMethod().GetString1()),
    ];

    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.MethodCall(
                node.AnyOne()
                    .Equals(typeof(string), e => e.Type)
                    .HavingAnyChild()
            ))
            .WithName("params"),

        ExpressionTreeEvaluator.Create(node => node.MethodCall(_ =>
            [
                node.AnyOne()
                    .Equals(typeof(string), e => e.Type)
                    .HavingAnyChild()
            ]))
            .WithName("lambda"),
        
        // TODO why did i think these were a good idea?
        // ExpressionTreeEvaluator.Create(node => node.MethodCall([]))
        //     .WithName("empty params"),
        
        // ExpressionTreeEvaluator.Create(node => node.MethodCall(_ => []))
        //     .WithName("empty lambda"),
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
