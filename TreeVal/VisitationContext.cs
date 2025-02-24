using System.Linq.Expressions;

namespace TreeVal;

public interface IVisitationTracker
{
    bool HasRejection { get; }

    void Accept(IEvaluatorNode visitor);
    void Reject(IEvaluatorNode visitor);
}

public interface IVisitationContext : IVisitationTracker
{
    bool CanMoveForward();
    Expression MoveForward();
    Expression? PeekForward();

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

    public bool CanMoveForward()
        => _head.CanMoveForward();

    public Expression MoveForward()
        => _head.MoveForward();

    public Expression? PeekForward()
        => _head.PeekForward();

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
