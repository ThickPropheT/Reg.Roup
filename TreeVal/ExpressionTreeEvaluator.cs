using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class ExpressionTreeEvaluator
{
    private readonly IEvaluatorNodeFactory _schema;

    public string? Name { get; private set; }

    private ExpressionTreeEvaluator(IEvaluatorNodeFactory schema)
    {
        _schema = schema;
    }

    public static ExpressionTreeEvaluator Create(Func<IVisitorNodeFactory, IEvaluatorNodeFactory> buildEvaluatorTree)
    {
        var factory = new DefaultVisitorNodeFactory();

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
