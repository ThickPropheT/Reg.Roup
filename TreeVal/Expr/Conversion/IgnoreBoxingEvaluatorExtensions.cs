using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Conversion;

public static class IgnoreBoxingEvaluatorExtensions
{
    public static IEvaluatorBuilderFactory IgnoreBoxing(this IEvaluatorBuilderFactory factory)
        => new BuilderFactoryAspect(factory, evaluator => new IgnoreBoxingEvaluator(evaluator));

    private class IgnoreBoxingEvaluator : INodeEvaluator
    {
        private readonly INodeEvaluator _target;

        public VisitationContext.MovementStrategy HeadMovementStrategy { get; }
        public VisitationContext.EvaluationStrategy? ChildEvaluationStrategy => _target.ChildEvaluationStrategy;

        public IgnoreBoxingEvaluator(INodeEvaluator target)
        {
            _target = target;

            HeadMovementStrategy = VisitationContext.MovementStrategy.From((context, head, evaluation) =>
            {
                var moveHead = _target.HeadMovementStrategy.GetStrategy(context, evaluation);

                var current = moveHead(head);

                return current?.Value is UnaryExpression { NodeType: ExpressionType.Convert }
                    ? moveHead(head)
                    : current;
            });
        }

        public IEnumerable<ICondition> EnumerateConditions(Node current)
            => _target.EnumerateConditions(current);

        public IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current)
            => _target.EnumerateChildren(current);
    }
}
