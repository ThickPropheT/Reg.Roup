namespace TreeVal;

public partial class VisitationContext
{
    public TapeHead Head { get; }

    public bool HasRejection { get; private set; }

    public VisitationContext(TapeHead head)
    {
        Head = head;
    }

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
}
