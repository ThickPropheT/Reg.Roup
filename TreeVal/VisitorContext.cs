using TreeVal.Media;

namespace TreeVal;

public class VisitorContext
{
    public TapeHead TapeHead { get; }
    
    public IEnumerable<VisitationResult> VisitationResults { get; }
}
