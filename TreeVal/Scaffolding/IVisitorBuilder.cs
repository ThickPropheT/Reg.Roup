using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public interface IVisitorBuilder : IVisitorFactory
{
    IVisitorBuilderFactory Originator { get; }
    
    IStageQuery<TStage> Get<TStage>()
        where TStage : IVisitationStageBuilder;
    
    IStageQuery<TStage> Get<TStage>(Key<TStage> key)
        where TStage : IVisitationStageBuilder;

    public class Key<T>
    {
        private readonly object _identity;

        public Key()
        {
            _identity = typeof(T);
        }

        public Key(object identity)
        {
            _identity = identity;
        }
    }

    public interface IStageQuery<TStage>
    {
        TStage? Stage();
        TStage OrCreateStage(Func<TStage>? createStage = null);
    }
}

public interface IVisitorBuilder<TNode> : IVisitorBuilder
{
}
