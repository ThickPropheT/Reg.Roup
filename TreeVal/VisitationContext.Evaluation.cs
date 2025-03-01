namespace TreeVal;

public partial class VisitationContext
{
    private readonly Dictionary<IEvaluatorNode, EvaluatorInfo> _nodeInfos = new();

    public bool HasRejection => _nodeInfos.Values.Any(s => s.Status == EvaluatorStatus.Rejected);

    public void Evaluate(IEvaluatorNodeFactory factory)
    {
        var evaluator = factory.ToEvaluator();
        var current = Head.MoveForward();

        try
        {
            var failedConditions = evaluator.Conditions.Where(c => !c.Evaluate(current)).ToArray();

            if (failedConditions.Any())
            {
                // TODO pass in failedConditions
                Reject(evaluator);
            }
        }
        catch (TreeRejectedException)
        {
            Reject(evaluator);
            return;
        }

        var evaluateChildren = evaluator.ChildEvaluationStrategy.GetStrategy(this);

        evaluateChildren(evaluator, current);

        TryAccept(evaluator);
    }

    public void TryAccept(IEvaluatorNode evaluator)
        => GetOrCreateStatusFor(evaluator).TryAccept();

    public void Reject(IEvaluatorNode evaluator)
        => GetOrCreateStatusFor(evaluator).Reject();

    private EvaluatorInfo GetOrCreateStatusFor(IEvaluatorNode evaluator)
        => !_nodeInfos.TryGetValue(evaluator, out var info)
            ? _nodeInfos[evaluator] = new EvaluatorInfo()
            : info;

    private void FastForwardStatuses(VisitationContext other)
    {
        foreach (var (key, value) in other._nodeInfos)
        {
            _nodeInfos[key] = value;
        }
    }

    private class EvaluatorInfo
    {
        public EvaluatorStatus Status { get; private set; } = EvaluatorStatus.Unknown;

        public void TryAccept()
        {
            if (Status == EvaluatorStatus.Rejected)
            {
                return;
            }

            Status = EvaluatorStatus.Accepted;
        }

        public void Reject()
            => Status = EvaluatorStatus.Rejected;
    }

    private enum EvaluatorStatus
    {
        Unknown = 0,
        Accepted,
        Rejected
    }
}
