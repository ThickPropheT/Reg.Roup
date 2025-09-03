using TreeVal.Eval;
using TreeVal.Extensions;
using TreeVal.Media;

namespace TreeVal;

public partial class VisitationContext
{
    private void EvaluateAll(INodeEvaluator evaluator, Node current, Evaluation evaluation)
    {
        var evaluated = evaluator
            .EnumerateChildren(current)
            .Select(c =>
            {
                var childEvaluation = new Evaluation();
                Evaluate(c, childEvaluation);
                return childEvaluation;
            })
            .TakeDoWhile(result => result.CurrentStatus == Evaluation.Status.Accepted)
            .ToArray();

        var rejected = evaluated.FirstOrDefault(result => result.CurrentStatus == Evaluation.Status.Rejected);

        if (rejected == null)
            return;

        evaluation.Reject(evaluated);
    }

    private void EvaluateAny(INodeEvaluator evaluator, Node current, Evaluation evaluation)
    {
        var evaluated = evaluator
            .EnumerateChildren(current)
            .Select(child =>
            {
                var clip = BranchFromHead();
                var childEvaluation = new Evaluation();
                clip.Evaluate(child, childEvaluation);
                return (clip, childEvaluation);
            })
            .TakeDoWhile(result => result.childEvaluation.CurrentStatus == Evaluation.Status.Rejected)
            .ToArray();

        var (accepted, _) = evaluated.FirstOrDefault(result =>
            result.childEvaluation.CurrentStatus == Evaluation.Status.Accepted);

        if (accepted != null)
        {
            accepted.SpliceOnto(this);
            return;
        }

        evaluation.Reject(evaluated.Select(e => e.childEvaluation));
    }

    public class EvaluationStrategy
    {
        public static EvaluationStrategy AllOf { get; } = new(context => context.EvaluateAll);
        public static EvaluationStrategy OneOf { get; } = new(context => context.EvaluateAny);

        private readonly Func<VisitationContext, Action<INodeEvaluator, Node, Evaluation>> _lookupStrategy;

        private EvaluationStrategy(
            Func<VisitationContext, Action<INodeEvaluator, Node, Evaluation>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }
        
        public Action<INodeEvaluator, Node, Evaluation> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
