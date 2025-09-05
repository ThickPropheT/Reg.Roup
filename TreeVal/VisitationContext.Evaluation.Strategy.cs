using TreeVal.Eval;
using TreeVal.Eval.All;
using TreeVal.Eval.Any;
using TreeVal.Extensions;
using TreeVal.Media;

namespace TreeVal;

public partial class VisitationContext
{
    private void EvaluateAll(INodeEvaluator evaluator, Node current, INodeEvaluation evaluation)
    {
        var evaluated = evaluator
            .EnumerateChildren(current)
            .Select(child =>
            {
                var childEvaluation = new DefaultNodeEvaluation(evaluation, child);
                Evaluate(childEvaluation);
                return childEvaluation;
            })
            .TakeDoWhile(result => result.Status == EvaluationStatus.Accepted)
            .ToArray();

        evaluation.RejectWhenAnyRejected(evaluated);
    }

    private void EvaluateAny(INodeEvaluator evaluator, Node current, INodeEvaluation evaluation)
    {
        var evaluated = evaluator
            .EnumerateChildren(current)
            .Select(child =>
            {
                var clip = BranchFromHead();
                var childEvaluation = new DefaultNodeEvaluation(evaluation, child);
                clip.Evaluate(childEvaluation);
                return (clip, childEvaluation);
            })
            .TakeUntil(result => result.childEvaluation.Status == EvaluationStatus.Accepted)
            .ToArray();

        var (accepted, _) = evaluated.FirstOrDefault(result =>
            result.childEvaluation.Status == EvaluationStatus.Accepted);

        accepted?.SpliceOnto(this);

        evaluation.RejectWhenAllRejected(evaluated.Select(e => e.childEvaluation));
    }

    public class EvaluationStrategy
    {
        public static EvaluationStrategy AllOf { get; } = new(context => context.EvaluateAll);
        public static EvaluationStrategy OneOf { get; } = new(context => context.EvaluateAny);

        private readonly Func<VisitationContext, Action<INodeEvaluator, Node, INodeEvaluation>> _lookupStrategy;

        private EvaluationStrategy(
            Func<VisitationContext, Action<INodeEvaluator, Node, INodeEvaluation>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        public Action<INodeEvaluator, Node, INodeEvaluation> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
