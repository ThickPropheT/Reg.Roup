namespace TreeVal.Scaffolding.Stage;

// ReSharper disable once UnusedTypeParameter
public class Accessors<TStage>
{
    public IVisitorBuilder Builder { get; }
    public IStageDirector StageDirector => Builder.StageDirector;

    public Accessors(IVisitorBuilder builder)
    {
        Builder = builder;
    }
}
