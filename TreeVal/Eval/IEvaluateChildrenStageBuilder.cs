using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public interface IEvaluateChildrenStageBuilder : IVisitationStageBuilder
{
    void AddChildren(Func<Node, IEnumerable<IVisitorFactory>> getChildren);
}