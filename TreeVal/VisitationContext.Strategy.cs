using System.Linq.Expressions;

namespace TreeVal;

public partial class VisitationContext
{
    private bool EvaluateAll(IEvaluatorNode evaluator, Expression current)
    {
        var children = evaluator.EnumerateChildren(current).ToArray();
        var evaluated = children.Select(Evaluate).ToArray();
        var result = evaluated.All(isAccepted => isAccepted);
        // TODO swap back to this when you're done debugging
        // var result = evaluator.EnumerateChildren(current).Select(Evaluate).All(isAccepted => isAccepted);
        return result;
    }

    private bool EvaluateAny(IEvaluatorNode evaluator, Expression current)
    {
        var accepted = evaluator.EnumerateChildren(current)
            .FirstOrDefault(child =>
            {
                var clip = BranchFromHead();

                clip.Evaluate(child);

                return clip.TrySpliceOnto(this);
            });

        return accepted != null;
    }

    public class EvaluationStrategy
    {
        public static EvaluationStrategy AllOf { get; } = new(context => context.EvaluateAll);
        public static EvaluationStrategy OneOf { get; } = new(context => context.EvaluateAny);

        public static EvaluationStrategy From(Func<VisitationContext, Expression, IEvaluatorNode, bool> strategy)
            => new(context => (evaluator, current) => strategy(context, current, evaluator));


        private readonly Func<VisitationContext, Func<IEvaluatorNode, Expression, bool>> _lookupStrategy;

        private EvaluationStrategy(Func<VisitationContext, Func<IEvaluatorNode, Expression, bool>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        public Func<IEvaluatorNode, Expression, bool> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
