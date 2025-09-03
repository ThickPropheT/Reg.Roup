using System.Linq.Expressions;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal;

public class ExpressionTreeEvaluator
{
    private readonly INodeEvaluatorFactory _schema;

    public string? Name { get; private set; }

    private ExpressionTreeEvaluator(INodeEvaluatorFactory schema)
    {
        _schema = schema;
    }

    public static ExpressionTreeEvaluator Create(Func<IEvaluatorBuilderFactory, INodeEvaluatorFactory> buildEvaluatorTree)
    {
        var factory = new DefaultEvaluatorBuilderFactory();

        var root = buildEvaluatorTree(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

        return new ExpressionTreeEvaluator(root);
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
        
        VisitationContext.EvaluateTree(head, _schema);
    }

    public override string? ToString()
        => Name ?? base.ToString();
}
