namespace TreeVal.Condition;

public class ConditionFailedException : Exception
{
    public ICondition Condition { get; }

    public ConditionFailedException(ICondition condition)
        : base("TODO") // TODO
    {
        Condition = condition;
    }

    public ConditionFailedException(ICondition condition, Exception inner) 
        : base("TODO", inner) // TODO
    {
        Condition = condition;
    }
}
