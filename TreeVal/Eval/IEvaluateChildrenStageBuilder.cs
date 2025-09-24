using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public interface IEvaluateChildrenStageBuilder : IVisitationStageBuilder
{
    void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren);
}
