using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public interface IEvaluatorNodeFactory
{
    IEvaluatorNode ToEvaluator();
}

// TODO figure out better naming alignment w/ IExpressionVisitorNode
public interface IEvaluatorBuilder : IEvaluatorNodeFactory
{
    void AddCondition(ICondition condition);
    void AddChildren(Func<Expression, IEnumerable<IEvaluatorNodeFactory>> getChildren);
}

public interface IEvaluatorBuilder<TNode> : IEvaluatorBuilder
{
    // TODO
    //  can this be merged into IEvaluatorBuilder above,
    //  replacing usages of IEvaluatorBuilder w/ IEvaluatorBuilder<Expression>?
    //  perhaps these two could/should just be concrete implementations now?
}

public class ExpressionTreeEvaluator
{
    private readonly IEvaluatorNode _rootNode;

    private ExpressionTreeEvaluator(IEvaluatorNode rootNode)
    {
        _rootNode = rootNode;
    }

    public static ExpressionTreeEvaluator Create(Func<VisitorNodeFactory, IEvaluatorBuilder> getNodes)
    {
        var factory = new VisitorNodeFactory();

        var root = getNodes(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

        return new ExpressionTreeEvaluator(root.ToEvaluator());
    }

    // TODO find a way to return the expression tree here
    public void Evaluate(Expression expressionTree)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(expressionTree).ToArray();
        var head = new TapeHead(tape);

        var context = new VisitationContext(head);

        try
        {
            _rootNode.Evaluate(context);
        }
        catch (Exception)
        {
            throw new TreeRejectedException();
        }

        if (context.HasRejection)
        {
            throw new TreeRejectedException();
        }

        if (context.CanMoveForward())
        {
            // TODO reevaluated whether this should be tree rejected and not some other ex type
            throw new TreeRejectedException();
        }
    }
}
