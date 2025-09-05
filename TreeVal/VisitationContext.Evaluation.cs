using TreeVal.Diagnostics;
using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal;

// TODO why is it called VisitationContext? could it be called something better?
public partial class VisitationContext
{
    public static void EvaluateTree(TapeHead head, INodeEvaluatorFactory schema)
    {
        var context = new VisitationContext(head);
        var evaluation = new DefaultNodeEvaluation(null, schema);

        try
        {
            context.Evaluate(evaluation);
        }
        catch (TreeRejectedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw TreeRejectedException.ForError(head, evaluation, ex);
        }

        if (evaluation.Status == EvaluationStatus.Rejected)
            throw TreeRejectedException.ForRejection(head, evaluation);

        if (head.CanMoveForward())
            throw TreeRejectedException.ForIncompleteRead(head, evaluation);
    }

    public void Evaluate(INodeEvaluation evaluation)
    {
        var evaluator = evaluation.GetEvaluator();
        var current = evaluation.GetTarget(this);

        if (current == null)
            return;

        // in the most ideal case, we'll want to iterate all the conditions below
        // for the purpose of evaluating them. may as well get it out of the way.
        var conditions = evaluator.Conditions.ToArray();
        var conditionEvaluations = new List<DefaultConditionEvaluation>(conditions.Length);

        try
        {
            foreach (var condition in conditions)
            {
                var conditionEvaluation = new DefaultConditionEvaluation(evaluation, condition, current);
                conditionEvaluations.Add(conditionEvaluation);

                condition.Evaluate(current, conditionEvaluation);
            }
        }
        catch (ConditionFailedException ex)
        {
            ex.Evaluation.Reject(ex);
        }

        evaluation.Record(conditionEvaluations);

        if (evaluation.Status == EvaluationStatus.Rejected)
            // don't bother evaluating children if a rejection has already occurred.
            return;

        var evaluateChildren = evaluator.ChildEvaluationStrategy?.GetStrategy(this);

        evaluateChildren?.Invoke(evaluator, current, evaluation);
    }
}
