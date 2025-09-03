using TreeVal.Media;

namespace TreeVal.Eval.Condition;

public class ConditionFailedException : Exception
{
    public ICondition Condition { get; }
    public Node Node { get; }

    public ConditionFailedException(ICondition condition, Node node)
        : base("TODO") // TODO
    {
        Condition = condition;
        Node = node;
    }

    public ConditionFailedException(ICondition condition, Node node, Exception inner) 
        : base("TODO", inner) // TODO
    {
        Condition = condition;
        Node = node;
    }
}
