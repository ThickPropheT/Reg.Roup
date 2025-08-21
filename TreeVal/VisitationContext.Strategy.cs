using System.Linq.Expressions;

namespace TreeVal;

public partial class VisitationContext
{
    private void EvaluateAll(IEvaluatorNode evaluator, Expression current)
    {
        if (evaluator.EnumerateChildren(current).Select(Evaluate).All(isAccepted => isAccepted))
            return;

        Reject(current, evaluator);
    }

    private void EvaluateAny(IEvaluatorNode evaluator, Expression current)
    {
        var accepted = evaluator.EnumerateChildren(current)
            .FirstOrDefault(child =>
            {
                var clip = BranchFromHead();

                clip.Evaluate(child);

                return clip.TrySpliceOnto(this);
            });

        if (accepted != null)
            return;

        Reject(current, evaluator); // TODO pass some explanation in here
    }

    public class EvaluationStrategy
    {
        public static EvaluationStrategy AllOf { get; } = new(context => context.EvaluateAll);
        public static EvaluationStrategy OneOf { get; } = new(context => context.EvaluateAny);

        public static EvaluationStrategy From(Action<VisitationContext, Expression, IEvaluatorNode> strategy)
            => new(context => (evaluator, current) => strategy(context, current, evaluator));


        private readonly Func<VisitationContext, Action<IEvaluatorNode, Expression>> _lookupStrategy;

        private EvaluationStrategy(Func<VisitationContext, Action<IEvaluatorNode, Expression>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        public Action<IEvaluatorNode, Expression> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
