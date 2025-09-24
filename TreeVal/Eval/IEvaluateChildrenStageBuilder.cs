using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public interface IVisitChildrenStageBuilder : IVisitationStageBuilder
{
    void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren);
}
