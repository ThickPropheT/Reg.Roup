namespace TreeVal.Stage.Eval;

public class ConditionFailedException : Exception
{
    public IConditionContext Context { get; }

    public ConditionFailedException(IConditionContext context, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        Context = context;
    }

    public static ConditionFailedException ExpectedNode<T>(IConditionContext context)
        => new(context, $"Node must have value of type {typeof(T)}");
}
