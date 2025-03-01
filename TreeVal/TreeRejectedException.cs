namespace TreeVal;

public class TreeRejectedException : Exception
{
    public EvaluationResult[] Trace { get; }

    public TreeRejectedException(EvaluationResult[] trace)
    {
        Trace = trace;
    }

    public TreeRejectedException(string message) : base(message)
    {
    }

    public TreeRejectedException(string message, Exception inner) : base(message, inner)
    {
    }
}
