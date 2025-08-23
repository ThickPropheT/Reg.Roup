using System.Linq.Expressions;
using TreeVal.Extensions;

namespace TreeVal.Tests;

[TestFixture]
public class AcceptChildren
{
    private static readonly ExpressionTreeEvaluator[] Evaluators =
    [
        ExpressionTreeEvaluator.Create(node => node.OfType<BinaryExpression>().AcceptChildren()),
        ExpressionTreeEvaluator.Create(node => node.OfType<BinaryExpression>().HavingChild(node.AcceptChildren))
        // ExpressionTreeEvaluator.Create(node => node.OfType<MethodCallExpression>().AcceptChildren())
    ];

    [TestFixtureSource(nameof(InvalidExpressions))]
    public class OnInvalidSchema
    {
        private static readonly Expression[] InvalidExpressions =
        [
            Expression.Constant(1),
            Expression.Convert(Expression.Constant(1), typeof(short))
        ];

        private readonly Expression _invalidExpression;

        public OnInvalidSchema(Expression invalidExpression)
        {
            _invalidExpression = invalidExpression;
        }

        [TestCaseSource(typeof(AcceptChildren), nameof(Evaluators))]
        public void Throw(ExpressionTreeEvaluator evaluator)
        {
            Assert.That(() => evaluator.Evaluate(_invalidExpression), Throws.TypeOf<TreeRejectedException>());
        }
    }

    [TestFixtureSource(nameof(ValidExpressions))]
    public class OnValidSchema
    {
        private static readonly Expression[] ValidExpressions =
        [
            Expression.Add(Expression.Constant(1), Expression.Constant(1)),
            Expression.Subtract(Expression.Constant(1), Expression.Constant(1)),
            Expression.Subtract(Expression.Multiply(Expression.Constant(1), Expression.Constant(1)), Expression.Constant(1))
            // Expression.Call(Expression.Property(null, typeof(C).GetProperty(nameof(C.Instance))!.GetMethod!), typeof(C).GetMethod(nameof(C.M))!, Expression.Constant("s"), Expression.Constant(1))
        ];

        private readonly Expression _validExpression;

        public OnValidSchema(Expression validExpression)
        {
            _validExpression = validExpression;
        }

        [TestCaseSource(typeof(AcceptChildren), nameof(Evaluators))]
        public void DoNotThrow(ExpressionTreeEvaluator evaluator)
        {
            Assert.That(() => evaluator.Evaluate(_validExpression), Throws.Nothing);
        }

        // private class C
        // {
        //     public static C Instance { get; } = new();
        //     
        //     public void M(string s, int i) { }
        // }
    }
}
