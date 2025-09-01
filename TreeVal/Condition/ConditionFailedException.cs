using System.Linq.Expressions;

namespace TreeVal.Condition;

public class ConditionFailedException : Exception
{
    public ICondition<Expression> Condition { get; }

    public ConditionFailedException(ICondition<Expression> condition)
        : base("TODO") // TODO
    {
        Condition = condition;
    }

    public ConditionFailedException(ICondition<Expression> condition, Exception inner) 
        : base("TODO", inner) // TODO
    {
        Condition = condition;
    }
}
