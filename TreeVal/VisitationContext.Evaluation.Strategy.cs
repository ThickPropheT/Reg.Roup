using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

public partial class VisitationContext
{
    private void EvaluateAll(INodeEvaluator evaluator, Node current, Evaluation evaluation)
    {
        var rejected = evaluator.EnumerateChildren(current)
            .Select(c =>
            {
                var childEvaluation = new Evaluation();
                Evaluate(c, childEvaluation);
                return childEvaluation;
            })
            .FirstOrDefault(childEvaluation => childEvaluation.CurrentStatus == Evaluation.Status.Rejected);

        if (rejected == null)
            return;

        evaluation.Reject();
    }

    private void EvaluateAny(INodeEvaluator evaluator, Node current, Evaluation evaluation)
    {
        var (accepted, _) = evaluator
            .EnumerateChildren(current)
            .Select(child =>
            {
                var clip = BranchFromHead();
                var childEvaluation = new Evaluation();
                clip.Evaluate(child, childEvaluation);
                return (clip, childEvaluation);
            })
            .FirstOrDefault(result => result.childEvaluation.CurrentStatus != Evaluation.Status.Rejected);

        if (accepted != null)
        {
            accepted.SpliceOnto(this);
            return;
        }

        evaluation.Reject();
    }

    public class EvaluationStrategy
    {
        public static EvaluationStrategy AllOf { get; } = new(context => context.EvaluateAll);
        public static EvaluationStrategy OneOf { get; } = new(context => context.EvaluateAny);

        // TODO this isn't used anymore. should it be removed?
        // public static EvaluationStrategy From(Action<VisitationContext, Node, IEvaluatorNode> strategy)
        //     => new(context => (evaluator, current, evaluation) => strategy(context, current, evaluator));


        private readonly Func<VisitationContext, Action<INodeEvaluator, Node, Evaluation>> _lookupStrategy;

        private EvaluationStrategy(
            Func<VisitationContext, Action<INodeEvaluator, Node, Evaluation>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        // TODO
        //  consider doing away with the indirection here and just give the strategy
        //  it's own "Do Strategy" method. i think i had it this way originally, b/c
        //  the Evaluate* methods above were responsible for accepting/rejecting things
        //  themselves, rather than having the ValidationContext be responsible for it.
        public Action<INodeEvaluator, Node, Evaluation> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
