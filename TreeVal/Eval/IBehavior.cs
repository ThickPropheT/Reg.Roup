using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval;

public interface IBehavior
{
    void Perform(BehaviorContext context);
}

public class TraverseMediaBehavior : IBehavior
{
    private readonly TapeHead _head;

    public TraverseMediaBehavior(TapeHead head)
    {
        _head = head;
    }
    
    public void Perform(BehaviorContext context)
    {
        
    }
}

public class Evaluate : IBehavior
{
    private readonly ICondition _condition;

    public Evaluate(ICondition condition)
    {
        _condition = condition;
    }
    
    public void Perform(BehaviorContext context)
    {
        
    }
}

public class VisitationBehavior : IBehavior
{
    private readonly IVisitor _visitor;

    public VisitationBehavior(IVisitor visitor)
    {
        _visitor = visitor;
    }
    
    public void Perform(BehaviorContext context)
    {
        
    }
}
