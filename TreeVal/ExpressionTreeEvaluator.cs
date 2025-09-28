using System.Linq.Expressions;
using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal;

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
        IDescriptionBuilder? descriptionBuilder = null)
    {
        var factory = new VisitorBuilderFactory(null);

        var root = buildEvaluatorTree(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

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

        var head = new TapeHead(tape);

        try
        {
            var v = new VisitationEngine();
            // VisitationContext.EvaluateTree(head, _schema);
        }
        catch (TreeRejectedException ex)
        {
            throw TreeRejectedException.Rethrow(ex, _descriptionBuilder);
        }
    }

    public override string? ToString()
        => Name ?? base.ToString();
}
