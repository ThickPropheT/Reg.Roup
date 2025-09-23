namespace TreeVal.Scaffolding;

public class BuilderObserver : VisitorBuilder
{
    protected BuilderObserver(IVisitorBuilderFactory originator)
        : base(originator)
    {
    }
}

public class BuilderObserver<TNode> : BuilderObserver, IVisitorBuilder<TNode>
{
    protected BuilderObserver(IVisitorBuilderFactory originator)
        : base(originator)
    {
    }
}
