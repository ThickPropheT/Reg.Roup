using System.Runtime.CompilerServices;
using TreeVal.Media;

namespace TreeVal.Scaffolding.Stage;

public interface IStageQuery<TStage>
{
    void Stage(Action<TStage?> callback);

    IVisitorBuilder OrCreateStage([CallerMemberName] string callerMemberName = "");
    IVisitorBuilder OrCreateStage(Func<Node, TStage> createStage);

    IVisitorBuilder OrCreateStage(Action<Node, TStage> callback, [CallerMemberName] string callerMemberName = "");
    IVisitorBuilder OrCreateStage(Action<Node, TStage> callback, Func<Node, TStage> createStage);
}
