using System.Linq.Expressions;
using TreeVal.Condition;
using TreeVal.Extensions2;

namespace TreeVal;

// TODO why is it called VisitationContext? could it be called something better?
public partial class VisitationContext
{
    // TODO
    //  - key by Expression & default EvaluationResult to Unread
    //     - ^^^? maybe to allow the expression tree to be the source of truth aka key off of
    //       the element being evaluated rather than the thing doing the evaluation and
    //       offer evaluation metadata as the value?
    //  - update accept/reject methods to also take Expression
    //    - ^^^? pass in the expression to the EvaluationResult to keep the target and metadata packaged together?
    private readonly Dictionary<IEvaluatorNode, EvaluationResult> _evaluationResults = new();

    private bool HasRejection => _evaluationResults.Values.Any(s => s.Status == EvaluatorStatus.Rejected);

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
            // TODO is wrapping ex providing value?
            // TODO should tape be passed in here?
            throw TreeRejectedException.ForError(ex);
        }

        var trace = context._evaluationResults.Values
            .TakeWhileInclusive(result => result.Status != EvaluatorStatus.Rejected)
            .ToArray();

        if (trace.Last().Status == EvaluatorStatus.Rejected)
        {
            throw TreeRejectedException.ForTrace(trace);
        }

        if (head.CanMoveForward())
        {
            throw TreeRejectedException.ForIncompleteRead(trace, head);
        }
    }

    public void Evaluate(IEvaluatorNodeFactory factory)
    {
        var evaluator = factory.ToEvaluator();
        var current = Head.MoveForward();

        // in the most ideal case, we'll want to iterate all the conditions below
        // for the purpose of evaluating them. may as well get it out of the way
        // and then be able to access it's length without multiple enumeration.
        var conditions = evaluator.Conditions.ToArray();
        var failedConditions = new List<ICondition>(conditions.Length);
        
        try
        {
            // keep failedConditions up-to-date as we evaluate
            failedConditions.AddRange(conditions.Where(c => !c.Evaluate(current)));

            if (failedConditions.Any())
            {
                ConditionsFailed(current, evaluator, failedConditions.ToArray());
            }
        }
        catch (ConditionFailedException ex)
        {
            // this one failed hard. add it to the pile
            failedConditions.Add(ex.Condition);
            ConditionsFailed(current, evaluator, failedConditions.ToArray());
            return;
        }

        var evaluateChildren = evaluator.ChildEvaluationStrategy.GetStrategy(this);

        evaluateChildren(evaluator, current);

        AcceptOrReject(current, evaluator);
    }

    /// <summary>
    /// TODO
    ///  why does this do what it does? can we and should we provide more context
    ///  through the Accept() & Reject() calls below? 
    /// </summary>
    /// <param name="node">TODO see TODOs above _evaluationResults</param>
    /// <param name="evaluator"></param>
    public void AcceptOrReject(Expression node, IEvaluatorNode evaluator)
    {
        var result = GetOrCreateEvaluationResult(evaluator);

        if (!HasRejection)
        {
            result.Accept();
        }
        else
        {
            result.Reject();
        }
    }

    public bool AcquiesceToPriorRejection(IEvaluatorNode evaluator)
    {
        if (HasRejection)
        {
            Reject(evaluator);
            return true;
        }

        return false;
    }

    public void Reject(IEvaluatorNode evaluator)
        => GetOrCreateEvaluationResult(evaluator).Reject();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node">TODO see TODOs above _evaluationResults</param>
    /// <param name="evaluator"></param>
    public void ConditionsFailed(Expression node, IEvaluatorNode evaluator, params ICondition[] failedConditions)
        => GetOrCreateEvaluationResult(evaluator).Reject(failedConditions);

    private EvaluationResult GetOrCreateEvaluationResult(IEvaluatorNode evaluator)
        => !_evaluationResults.TryGetValue(evaluator, out var result)
            ? _evaluationResults[evaluator] = new EvaluationResult(evaluator)
            : result;

    private void FastForwardStatuses(VisitationContext other)
    {
        foreach (var result in other._evaluationResults.Values)
        {
            _evaluationResults[result.Evaluator] = result;
        }
    }
}

public class EvaluationResult : IDescribable
{
    public IEvaluatorNode Evaluator { get; }
    public EvaluatorStatus Status { get; private set; } = EvaluatorStatus.Unknown;
    public ICondition[] FailedConditions { get; private set; } = [];

    public EvaluationResult(IEvaluatorNode evaluator)
    {
        Evaluator = evaluator;
    }

    public void Accept()
        => Status = EvaluatorStatus.Accepted;

    public void Reject()
        => Status = EvaluatorStatus.Rejected;

    public void Reject(ICondition[] failedConditions)
    {
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
    Unknown = 0, // TODO change this to Unread?
    Accepted,
    Rejected
}
