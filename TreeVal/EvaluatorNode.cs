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

    public void Evaluate(IVisitationContext context)
    {
        var current = context.MoveForward();

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

        // TODO add auto-accept and remove this
        context.Accept(this);
    }

    protected virtual void EvaluateConditions(IVisitationContext context, Expression current)
    {
        var failedConditions = _conditions.Where(c => !c.Evaluate(current)).ToArray();

        if (failedConditions.Any())
        {
            // TODO pass in failedConditions
            context.Reject(this);
        }
    }

    protected virtual void EvaluateChildren(IVisitationContext context, Expression current)
    {
        foreach (var child in _childLookups.SelectMany(lookup => lookup(current)))
        {
            var visitor = child.ToEvaluator();
            visitor.Evaluate(context);

            if (context.HasRejection)
            {
                context.Reject(this);
                return;
            }
        }
    }
}
