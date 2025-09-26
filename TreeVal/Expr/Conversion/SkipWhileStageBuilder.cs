using TreeVal.Eval;
using TreeVal.Media;

namespace TreeVal.Expr.Conversion;

public class SkipWhileStageBuilder : VisitationStageBuilder, IReadNodeStageBuilder
{
    public SkipWhileStageBuilder(Func<Node, bool> predicate)
        : base(new IVisitationStageBuilder.Identity<IReadNodeStageBuilder>())
    {
        AfterEntering(_ => new MediaBehavior.SkipWhile(predicate));
    }
}
