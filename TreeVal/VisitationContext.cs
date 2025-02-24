using System.Linq.Expressions;

namespace TreeVal;

public interface IVisitationTracker
{
    bool HasRejection { get; }
    
    void Accept(IEvaluatorNode visitor);
    void Reject(IEvaluatorNode visitor);
}

// TODO evaluate naming
public interface IVisitationContext : IVisitationTracker
{
    // TODO evaluate call sites where nullability can be ignored
    Expression? ReadCurrent();

    bool CanMoveForward();
    Expression? MoveForward();
    
    bool CanMoveBackward();
    Expression? MoveBackward();

    IVisitationTracker Try(Action<IVisitationContext> scope);
}

public class VisitationContext : IVisitationContext
{
    private readonly ITapeHead _head;
    
    public bool HasRejection { get; private set; }

    public VisitationContext(ITapeHead head)
    {
        _head = head;
    }

    public Expression? ReadCurrent()
        => _head.Read();

    public bool CanMoveForward()
        => _head.CanMoveForward();

    public Expression? MoveForward()
        => _head.MoveForward();

    public bool CanMoveBackward()
        => _head.CanMoveBackward();

    public Expression? MoveBackward()
        => _head.MoveBackward();

    public void Accept(IEvaluatorNode visitor)
    {
        // TODO
        //  is this even useful? I can't think of anything interesting to use this for
        //  maybe just assume accepted unless rejected?
        //  maybe build graph of what was accepted and rejected?
    }

    public void Reject(IEvaluatorNode visitor)
    {
        // TODO start actually tracking what failed
        HasRejection = true;
    }

    public IVisitationTracker Try(Action<IVisitationContext> scope)
    {
        var checkpoint = _head.Checkpoint();
        var copy = new VisitationContext(checkpoint.Head);

        scope(copy);

        if (!copy.HasRejection)
        {
            checkpoint.Commit();
        }

        return copy;
    }
}
