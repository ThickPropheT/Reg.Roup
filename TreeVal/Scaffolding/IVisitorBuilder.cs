using TreeVal.Eval;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public interface IVisitorBuilder : IVisitorFactory
{
    IVisitorBuilderFactory Originator { get; }

    void OnDiscovery(Action<Node, IDiscovered> callback);

    interface IDiscovered
    {
        IVisitationStageBuilder? Get(IVisitationStageBuilder.Identity key);
        void Set(IVisitationStageBuilder builder);
    }
}

public interface IVisitorBuilder<TNode> : IVisitorBuilder
{
}
