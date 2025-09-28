using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Stage.Children;
using TreeVal.Stage.Eval;
using TreeVal.Stage.Read;
using TreeVal.Visit;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

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

        var head = new InitialTapeHead(new TapeHead(tape));
        var visitorContext = new VisitorContext(head);

        var visitor = _schema.CreateVisitor(head.Read());

        visitor.Visit(visitorContext);

        if (IsRejected(visitorContext))
            throw TreeRejectedException.ForRejection(head, null);

        if (head.CanMoveForward)
            throw TreeRejectedException.ForIncompleteRead(head, null);

        // try
        // {
        //     var v = new VisitationEngine();
        //     // VisitationContext.EvaluateTree(head, _schema);
        // }
        // catch (TreeRejectedException ex)
        // {
        //     throw TreeRejectedException.Rethrow(ex, _descriptionBuilder);
        // }
    }

    public override string? ToString()
        => Name ?? base.ToString();

    private static bool IsRejected(VisitorContext visitorContext)
        => visitorContext.StageVisitations.Any(stageResult => IsRejected(stageResult));

    private static bool IsRejected(StageVisitationResult stageResult)
        => stageResult
               .Status == EvaluationStatus.Rejected
           || stageResult.StageContext.BehaviorVisitations.Any(behaviorResult => IsRejected(behaviorResult));

    private static bool IsRejected(BehaviorVisitationResult behaviorResult)
        => behaviorResult.Error != null
           || behaviorResult.BehaviorContext.VisitationResult.Error != null
           || behaviorResult.BehaviorContext.VisitationResult is ConditionEvaluationResult
           {
               Status: EvaluationStatus.Rejected
           };

    private class InitialTapeHead : ITapeHead
    {
        private readonly TapeHead _inner;

        public InitialTapeHead(TapeHead inner)
        {
            _inner = inner;
        }

        public bool CanMoveForward => _inner.CanMoveForward;

        public Node Read()
        {
            if (_inner.CanRead)
                return _inner.Read();
            
            if (_inner is { IsBeforeFront: true, CanMoveForward: true })
                return _inner.PeekForward()!;

            // TODO
            throw new InvalidOperationException();
        }

        public IEnumerable<Node> ReadToStart()
            => _inner.ReadToStart();

        public Node MoveForward()
            => _inner.MoveForward();

        public Node? PeekForward()
            => _inner.PeekForward();

        public void Describe(IDescriptionBuilder descriptionBuilder)
            => _inner.Describe(descriptionBuilder);
    }
}
