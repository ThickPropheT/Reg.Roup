using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Children;

public interface IVisitChildrenStageBuilder : IVisitationStageBuilder
{
    void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren);
}
