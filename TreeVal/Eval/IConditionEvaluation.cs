using TreeVal.Diagnostics;

namespace TreeVal.Eval;

public interface IConditionEvaluation : IDescribable
{
    INodeEvaluation Owner { get; }
    
    EvaluationStatus Status { get; }

    void Reject(Exception? error = null);
}
