using TreeVal.Visit;

namespace TreeVal.Stage.Children;

public class VisitorResolutionErrorResult : VisitationResult
{
    public VisitorResolutionErrorResult(Exception error)
    {
        Error = error;
    }
}
