using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Children;

public class Visit : IBehavior
{
    private readonly IVisitorFactory _child;
    private readonly Node _node;

    public Visit(IVisitorFactory child, Node node)
    {
        _child = child;
        _node = node;
    }

    public void Perform(BehaviorContext context)
    {
        throw new NotImplementedException();
        // _child.CreateVisitor(_node).Visit();
    }
}
