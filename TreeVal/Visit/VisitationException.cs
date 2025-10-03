using TreeVal.Media;

namespace TreeVal.Visit;

public class VisitationException : Exception
{
    public ITapeHead Head { get; }

    public IVisitorContext VisitorContext { get; }
    public VisitationResult Result { get; }

    protected VisitationException(Exception inner, IVisitorContext visitorContext, VisitationResult result)
        : base("Error occurred during the visitation of a node.", inner)
    {
        Head = visitorContext.TapeHead;
        VisitorContext = visitorContext;
        Result = result;
    }
}
