using System.Linq.Expressions;

namespace TreeVal;

public partial class VisitationContext
{
    public void Evaluate(IEvaluatorNodeFactory factory)
    {
        var evaluator = factory.ToEvaluator();
        var current = Head.MoveForward();

        try
        {
            var failedConditions = evaluator.Conditions.Where(c => !c.Evaluate(current)).ToArray();

            if (failedConditions.Any())
            {
                // TODO pass in failedConditions
                Reject(evaluator);
            }
        }
        catch (TreeRejectedException)
        {
            Reject(evaluator);
            return;
        }

        var evaluateChildren = evaluator.ChildEvaluationStrategy.GetStrategy(this);

        evaluateChildren(evaluator, current);

        if (!HasRejection)
        {
            Accept(evaluator);
        }
    }

    private void EvaluateAll(IEvaluatorNode evaluator, Expression current)
    {
        foreach (var child in evaluator.EnumerateChildren(current))
        {
            Evaluate(child);

            if (HasRejection)
            {
                Reject(evaluator);
                return;
            }
        }
    }

    private void EvaluateAny(IEvaluatorNode evaluator, Expression current)
    {
        var accepted = evaluator.EnumerateChildren(current)
            .FirstOrDefault(child =>
            {
                var branch = CreateBranch();

                branch.Evaluate(child);

                return branch.TryMerge();
            });

        if (accepted == null)
        {
            Reject(evaluator);
        }
    }

    private void Apply(Action<VisitationContext, Expression> strategy, IEvaluatorNode evaluator, Expression current)
    {
        strategy(this, current);

        if (HasRejection)
        {
            Reject(evaluator);
        }
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
