using TreeVal.Media;

namespace TreeVal.Scaffolding.Stage.Create;

public interface ICreateStage<TStage>
{
    IVisitorBuilder OrUpdate(Func<Node, TStage> createStage);
    IVisitorBuilder OrUpdate(Action<Node, TStage> callback, Func<Node, TStage> createStage);
}
