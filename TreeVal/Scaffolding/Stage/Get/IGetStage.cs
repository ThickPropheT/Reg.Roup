using System.Runtime.CompilerServices;
using TreeVal.Media;

namespace TreeVal.Scaffolding.Stage.Get;

public interface IGetStage<TStage>
{
    void Stage(Action<TStage?> callback);

    IVisitorBuilder OrCreate([CallerMemberName] string callerMemberName = "");
    IVisitorBuilder OrCreate(Func<Node, TStage> createStage);

    IVisitorBuilder OrCreate(Action<Node, TStage> callback, [CallerMemberName] string callerMemberName = "");
    IVisitorBuilder OrCreate(Action<Node, TStage> callback, Func<Node, TStage> createStage);
}
