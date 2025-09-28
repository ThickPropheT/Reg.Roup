using TreeVal.Visit;

namespace TreeVal.Stage.Children;

public class ChildVisitationResult : VisitationResult
{
    public IVisitorContext VisitorContext { get; }

    public ChildVisitationResult(IVisitorContext visitorContext)
    {
        VisitorContext = visitorContext;
    }

    public static ChildVisitationResult ForError(IVisitorContext visitorContext, Exception error)
        => new(visitorContext) { Error = error };
}
