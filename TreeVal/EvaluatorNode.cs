using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class EvaluatorNode : IEvaluatorNode
{
    private readonly IEnumerable<ICondition> _conditions;
    private readonly IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups;

    public EvaluatorNode(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
    {
        _conditions = conditions;
        _childLookups = childLookups;
    }

    public void Evaluate(VisitationContext context)
    {
        var current = context.Head.MoveForward();

        try
        {
            EvaluateConditions(context, current);
        }
        catch (TreeRejectedException)
        {
            context.Reject(this);
            return;
        }

        EvaluateChildren(context, current);
    }

    protected virtual void EvaluateConditions(VisitationContext context, Expression current)
    {
        var failedConditions = _conditions.Where(c => !c.Evaluate(current)).ToArray();

        if (failedConditions.Any())
        {
            // TODO pass in failedConditions
            context.Reject(this);
        }
    }

    protected virtual void EvaluateChildren(VisitationContext context, Expression current)
    {
        foreach (var child in _childLookups.SelectMany(lookup => lookup(current)))
        {
            var evaluator = child.ToEvaluator();
            evaluator.Evaluate(context);

            if (!context.HasRejection)
            {
                context.Accept(evaluator);
            }
            else
            {
                context.Reject(this);
                return;
            }
        }
    }
}
