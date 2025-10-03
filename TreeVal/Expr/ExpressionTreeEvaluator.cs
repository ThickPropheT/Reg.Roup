using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Extensions;
using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Stage.Children;
using TreeVal.Stage.Eval;
using TreeVal.Stage.Eval.Rejection;
using TreeVal.Stage.Read;
using TreeVal.Visit;

namespace TreeVal.Expr;

public class ExpressionTreeEvaluator
{
    private readonly IVisitorFactory _schema;
    private readonly IDescriptionBuilder? _descriptionBuilder;

    public string? Name { get; private set; }

    private ExpressionTreeEvaluator(IVisitorFactory schema, IDescriptionBuilder? descriptionBuilder)
    {
        _schema = schema;
        _descriptionBuilder = descriptionBuilder;
    }

    public static ExpressionTreeEvaluator Create(
        Func<IVisitorBuilderFactory, IVisitorFactory> buildEvaluatorTree,
        IDescriptionBuilder? descriptionBuilder = null
    )
    {
        var director = new StageDirector
        {
            [ReadNodeStage.Key] = () => new MoveForwardStageBuilder(),
            [EvaluateConditionsStage.Key] = () => new EvaluateConditionsStageBuilder(),
            [VisitChildrenStage.Key] = () => new VisitChildrenStageBuilder()
        };

        var factory = new VisitorBuilderFactory(director);
        var root = buildEvaluatorTree(factory);

        return new ExpressionTreeEvaluator(root, descriptionBuilder);
    }

    public ExpressionTreeEvaluator WithName(string name)
    {
        Name = name;
        return this;
    }

    public void Evaluate(Expression expressionTree)
    {
        var tape = LinearExpressionTreeRecorder
            .RecordVisitationOf(expressionTree)
            .ToArray<Node>();

        var head = new TapeHeadBootstrapper(new TapeHead(tape));

        var rejections = Array.Empty<VisitationResult>();

        IVisitor visitor;

        try
        {
            visitor = _schema.CreateVisitor(head.Read());
        }
        catch (Exception ex)
        {
            throw new NotImplementedException();
            
            // TODO figure out a permanent error solution here
            throw TreeRejectedException.ForError(head, null, ex);
        }

        var visitorContext = new VisitorContext(head, visitor);

        try
        {
            visitor.Visit(visitorContext);

            rejections = visitorContext
                .FindRejections()
                .ToArray();
        }
        catch (VisitationException ex)
            when (ex.Enumerate().Any(e => e is IndexOutOfRangeException))
        {
            throw TreeRejectedException.ForReadPastEnd(head, visitorContext);
        }
        catch (VisitationException ex)
        {
            throw TreeRejectedException.Rethrow(ex, _descriptionBuilder);
        }

        if (rejections.Any())
            throw TreeRejectedException.ForRejection(head, visitorContext);

        if (head.CanMoveForward)
            throw TreeRejectedException.ForIncompleteRead(head, visitorContext);
    }

    public override string? ToString()
        => Name ?? base.ToString();

    private class TapeHeadBootstrapper : ITapeHead
    {
        private readonly TapeHead _inner;

        private Func<Node> _read;

        public bool CanMoveForward => _inner.CanMoveForward;

        public TapeHeadBootstrapper(TapeHead inner)
        {
            _inner = inner;
            _read = BootstrapRead;
        }

        public Node Read() => _read();

        private Node BootstrapRead()
            => _inner.PeekForward()
               ?? throw new IndexOutOfRangeException();

        private Node NormalRead()
            => _inner.Read();

        public IEnumerable<Node> ReadToStart()
            => _inner.ReadToStart();

        public Node MoveForward()
        {
            _read = NormalRead;
            return _inner.MoveForward();
        }

        public Node? PeekForward()
            => _inner.PeekForward();

        public void Describe(IDescriptionBuilder descriptionBuilder)
            => _inner.Describe(descriptionBuilder);
    }
}
