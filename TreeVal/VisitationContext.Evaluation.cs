using TreeVal.Condition;
using TreeVal.Extensions2;

namespace TreeVal;

public partial class VisitationContext
{
    private readonly Dictionary<IEvaluatorNode, EvaluationResult> _evaluationResults = new();

    private bool HasRejection => _evaluationResults.Values.Any(s => s.Status == EvaluatorStatus.Rejected);

    public void Evaluate(IEvaluatorNodeFactory factory)
    {
        var evaluator = factory.ToEvaluator();
        var current = Head.MoveForward();

        try
        {
            var failedConditions = evaluator.Conditions.Where(c => !c.Evaluate(current)).ToArray();

            if (failedConditions.Any())
            {
                Reject(evaluator, failedConditions);
            }
        }
        catch (ConditionFailedException ex)
        {
            Reject(evaluator, ex.Condition);
            return;
        }

        var evaluateChildren = evaluator.ChildEvaluationStrategy.GetStrategy(this);

        evaluateChildren(evaluator, current);

        AcceptOrReject(evaluator);
    }

    public void AcceptOrReject(IEvaluatorNode evaluator)
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

    public bool TryReject(IEvaluatorNode evaluator)
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

    public void Reject(IEvaluatorNode evaluator, params ICondition[] failedConditions)
        => GetOrCreateEvaluationResult(evaluator).Reject(failedConditions);

    public void AssertNoRejections()
    {
        var trace = _evaluationResults.Values
            .TakeWhileInclusive(result => result.Status != EvaluatorStatus.Rejected)
            .ToArray();

        if (trace.Last().Status == EvaluatorStatus.Rejected)
        {
            throw new TreeRejectedException(trace);
        }

        if (Head.CanMoveForward())
        {
            // TODO better error message than this
            throw new TreeRejectedException("CanMoveForward");
        }
    }

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
    Unknown = 0,
    Accepted,
    Rejected
}
