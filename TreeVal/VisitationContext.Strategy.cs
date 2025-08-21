using System.Linq.Expressions;

namespace TreeVal;

public partial class VisitationContext
{
    private void EvaluateAll(IEvaluatorNode evaluator, Expression current)
    {
        foreach (var child in evaluator.EnumerateChildren(current))
        {
            Evaluate(child);

            if (AcquiesceToPriorRejection(evaluator))
            {
                return;
            }
        }
    }

    private void EvaluateAny(IEvaluatorNode evaluator, Expression current)
    {
        var accepted = evaluator.EnumerateChildren(current)
            .FirstOrDefault(child =>
            {
                var clip = StartClip();

                clip.Evaluate(child);

                return clip.TrySpliceOnto(this);
            });

        if (accepted == null)
        {
            Reject(evaluator); // TODO pass some explanation in here
        }
    }

    private void Apply(Action<VisitationContext, Expression> strategy, IEvaluatorNode evaluator, Expression current)
    {
        strategy(this, current);
        AcquiesceToPriorRejection(evaluator);
    }
    
    public class EvaluationStrategy
    {
        public static EvaluationStrategy AllOf { get; } = new(context => context.EvaluateAll);
        public static EvaluationStrategy OneOf { get; } = new(context => context.EvaluateAny);

        public static EvaluationStrategy From(Action<VisitationContext, Expression> strategy)
            => new(context => (evaluator, current) => context.Apply(strategy, evaluator, current));


        private readonly Func<VisitationContext, Action<IEvaluatorNode, Expression>> _lookupStrategy;

        private EvaluationStrategy(Func<VisitationContext, Action<IEvaluatorNode, Expression>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        public Action<IEvaluatorNode, Expression> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
