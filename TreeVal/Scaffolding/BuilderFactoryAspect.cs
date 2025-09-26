using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class BuilderFactoryAspect : IVisitorBuilderFactory
{
    private readonly IVisitorBuilderFactory _source;
    private readonly Func<IVisitorBuilder, IVisitorBuilder> _pipe;

    public IStageDirector StageDirector => _source.StageDirector;

    public BuilderFactoryAspect(IVisitorBuilderFactory source, Func<IVisitorBuilder, IVisitorBuilder> pipe)
    {
        _source = source;
        _pipe = pipe;
    }

    public IVisitorBuilder Where(Func<Node, bool> predicate, string predicateExpression = "")
        => _pipe(_source.Where(predicate, predicateExpression));

    public IVisitorBuilder<T> OfType<T>()
        => (IVisitorBuilder<T>) _pipe(_source.OfType<T>());

    public IVisitorBuilder OneOf(
        IVisitorFactory option1, IVisitorFactory option2, params IVisitorFactory[] options)
        => _pipe(_source.OneOf(option1, option2, options));
}
