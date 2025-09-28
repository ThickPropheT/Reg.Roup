using TreeVal.Media;

namespace TreeVal.Scaffolding.Stage;

public interface IStageQuery<TStage>
{
    void Stage(Action<TStage?> callback);
    IVisitorBuilder OrCreateStage(Func<Node, TStage>? createStage = null);
    IVisitorBuilder OrCreateStage(Action<Node, TStage> callback, Func<Node, TStage>? createStage = null);
}