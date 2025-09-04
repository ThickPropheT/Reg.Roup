namespace TreeVal.Eval.Condition;

public class ConditionFailedException : Exception
{
    public IConditionEvaluation Evaluation { get; }

    public ConditionFailedException(IConditionEvaluation evaluation, string message)
        : base(message)
    {
        Evaluation = evaluation;
    }

    public static ConditionFailedException ExpectedNode<T>(IConditionEvaluation evaluation)
        => new(evaluation, $"Node must have value of type {typeof(T)}");
}
