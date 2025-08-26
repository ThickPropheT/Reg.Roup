using System.Diagnostics;
using System.Linq.Expressions;
using TreeVal.Condition;
using TreeVal.Extensions2;

namespace TreeVal;

// TODO why is it called VisitationContext? could it be called something better?
public partial class VisitationContext
{
    private readonly Dictionary<IEvaluatorNode, EvaluationResult> _evaluationResults = new();

    public bool IsAnyResultRejected => _evaluationResults.Values.Any(s => s.Status == EvaluatorStatus.Rejected);

    public static void EvaluateTree(Expression expressionTree, IEvaluatorNodeFactory schema)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(expressionTree).ToArray();
        var head = new TapeHead(tape);

        var context = new VisitationContext(head);

        try
        {
            context.Evaluate(schema);
        }
        catch (TreeRejectedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw TreeRejectedException.ForError(GetTrace(context), head, ex);
        }

        var trace = GetTrace(context);

        if (trace.LastOrDefault()?.Status == EvaluatorStatus.Rejected)
        {
            throw TreeRejectedException.ForTrace(trace);
        }

        if (head.CanMoveForward())
        {
            throw TreeRejectedException.ForIncompleteRead(trace, head);
        }
    }

    public bool Evaluate(IEvaluatorNodeFactory factory)
    {
        var evaluator = factory.ToEvaluator();

        var moveHead = evaluator.HeadMovementStrategy.GetStrategy(this);
        
        var current = moveHead(Head);

        if (current == null)
        {
            return TryAccept(evaluator, current);
        }

        // in the most ideal case, we'll want to iterate all the conditions below
        // for the purpose of evaluating them. may as well get it out of the way
        // and then be able to access its length without multiple enumeration.
        var conditions = evaluator.Conditions.ToArray();
        var failedConditions = new List<ICondition>(conditions.Length);

        try
        {
            // keep failedConditions up-to-date as we evaluate
            failedConditions.AddRange(conditions.Where(c => !c.Evaluate(current)));

            if (failedConditions.Any())
            {
                Reject(evaluator, current, failedConditions.ToArray());
            }
        }
        catch (ConditionFailedException ex)
        {
            // this one failed hard. add it to the pile
            failedConditions.Add(ex.Condition);
            Reject(evaluator, current, failedConditions.ToArray());
            return false;
        }

        var wereAnyResultsRejected = IsAnyResultRejected;

        var evaluateChildren = evaluator.ChildEvaluationStrategy.GetStrategy(this);

        var areAllAccepted = evaluateChildren(evaluator, current);

        // TODO this might be ok to do actually. "evaluateChildren" isn't 100% accurate. try switching back to keying result dictionary on eval result and re evaluate this asseertion
        Debug.Assert(wereAnyResultsRejected == IsAnyResultRejected || !areAllAccepted,
            $"Should children be able to affect rejection status on parent? {wereAnyResultsRejected} -> {IsAnyResultRejected}");

        if (!areAllAccepted)
        {
            Reject(evaluator, current);
        }

        return TryAccept(evaluator, current);
    }

    public bool TryAccept(IEvaluatorNode evaluator, Expression? current)
    {
        var result = GetOrCreateEvaluationResult(evaluator, current);

        // TODO
        //  the fact that this status check is now necessary is sort of a harbinger
        //  that there may be some node-evaluator incongruency or identity consistencies 
        if (IsAnyResultRejected || result.Status != EvaluatorStatus.Unread)
            return false;

        result.Accept();
        return true;
    }

    public void Reject(IEvaluatorNode evaluator, Expression current)
        => GetOrCreateEvaluationResult(evaluator, current).Reject();

    public void Reject(IEvaluatorNode evaluator, Expression current, params ICondition[] failedConditions)
        => GetOrCreateEvaluationResult(evaluator, current).Reject(failedConditions);

    private EvaluationResult GetOrCreateEvaluationResult(IEvaluatorNode evaluator, Expression? current)
        => !_evaluationResults.TryGetValue(evaluator, out var result)
            ? _evaluationResults[evaluator] = new EvaluationResult(current, evaluator)
            : result;

    private void FastForwardStatuses(VisitationContext other)
    {
        foreach (var result in other._evaluationResults.Values)
        {
            _evaluationResults[result.Evaluator] = result;
        }
    }

    private static EvaluationResult[] GetTrace(VisitationContext context)
        => context._evaluationResults.Values
            .TakeWhileInclusive(result => result.Status != EvaluatorStatus.Rejected)
            .ToArray();
}

public class EvaluationResult : IDescribable
{
    public Expression? Node { get; }
    public IEvaluatorNode Evaluator { get; }
    public EvaluatorStatus Status { get; private set; } = EvaluatorStatus.Unread;
    public ICondition[] FailedConditions { get; private set; } = [];

    public EvaluationResult(Expression? node, IEvaluatorNode evaluator)
    {
        Node = node;
        Evaluator = evaluator;
    }

    public void Accept()
    {
        Debug.Assert(Status == EvaluatorStatus.Unread, $"Should mutable status be allowed? {Status} -> Accepted");
        Status = EvaluatorStatus.Accepted;
    }

    public void Reject()
    {
        Debug.Assert(Status == EvaluatorStatus.Unread, $"Should mutable status be allowed? {Status} -> Rejected");
        Status = EvaluatorStatus.Rejected;
    }

    public void Reject(ICondition[] failedConditions)
    {
        Debug.Assert(Status == EvaluatorStatus.Unread, $"Should mutable status be allowed? {Status} -> Rejected");
        Status = EvaluatorStatus.Rejected;
        FailedConditions = failedConditions;
    }

    public void Describe(IDescription description)
    {
        description.EmitResult(Status, Evaluator, FailedConditions);
    }
}

public enum EvaluatorStatus
{
    Unread = 0,
    Accepted,
    Rejected
}
