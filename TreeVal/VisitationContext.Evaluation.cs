using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

// TODO why is it called VisitationContext? could it be called something better?
public partial class VisitationContext
{
    public static void EvaluateTree(TapeHead head, INodeEvaluatorFactory schema)
    {
        var context = new VisitationContext(head);

        var evaluation = new Evaluation();

        try
        {
            context.Evaluate(schema, evaluation);
        }
        catch (TreeRejectedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw TreeRejectedException.ForError(head, ex);
        }

        if (evaluation.CurrentStatus == Evaluation.Status.Rejected)
            throw TreeRejectedException.ForTrace();

        if (head.CanMoveForward())
            throw TreeRejectedException.ForIncompleteRead(head);
    }

    public void Evaluate(INodeEvaluatorFactory factory, Evaluation evaluation)
    {
        var evaluator = factory.ToEvaluator();

        var moveHead = evaluator.HeadMovementStrategy.GetStrategy(this);

        var current = moveHead(Head);

        if (current == null)
            return;

        // in the most ideal case, we'll want to iterate all the conditions below
        // for the purpose of evaluating them. may as well get it out of the way.
        var conditions = evaluator.Conditions.ToArray();

        try
        {
            foreach (var condition in conditions)
            {
                condition.Evaluate(current, evaluation);
            }
        }
        catch (ConditionFailedException ex)
        {
            evaluation.Reject();
            return;
        }

        if (evaluation.CurrentStatus == Evaluation.Status.Rejected)
            // don't bother evaluating children if a rejection has already occurred.
            return;

        var evaluateChildren = evaluator.ChildEvaluationStrategy?.GetStrategy(this);

        evaluateChildren?.Invoke(evaluator, current, evaluation);
    }
}
