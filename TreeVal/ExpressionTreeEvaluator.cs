using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public interface IEvaluatorNodeFactory
{
    IEvaluatorNode ToEvaluator();
}

public interface IEvaluatorConditionBuilder : IEvaluatorNodeFactory
{
    void AddCondition(ICondition condition);
}

public interface IEvaluatorBuilder : IEvaluatorConditionBuilder
{
    void AddChildren(Func<Expression, IEnumerable<IEvaluatorNodeFactory>> getChildren);
}

public interface IEvaluatorConditionBuilder<TNode> : IEvaluatorNodeFactory
{
}

public interface IEvaluatorBuilder<TNode> : IEvaluatorBuilder, IEvaluatorConditionBuilder<TNode>
{
    // TODO
    //  can this be merged into IEvaluatorBuilder above,
    //  replacing usages of IEvaluatorBuilder w/ IEvaluatorBuilder<Expression>?
    //  perhaps these two could/should just be concrete implementations now?
}

public class ExpressionTreeEvaluator
{
    private readonly IEvaluatorNodeFactory _rootNode;

    private ExpressionTreeEvaluator(IEvaluatorNodeFactory rootNode)
    {
        _rootNode = rootNode;
    }

    public static ExpressionTreeEvaluator Create(Func<VisitorNodeFactory, IEvaluatorNodeFactory> getNodes)
    {
        var factory = new VisitorNodeFactory();

        var root = getNodes(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

        return new ExpressionTreeEvaluator(root);
    }

    public void Evaluate(Expression expressionTree)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(expressionTree).ToArray();
        var head = new TapeHead(tape);

        var context = new VisitationContext(head);

        try
        {
            context.Evaluate(_rootNode);
        }
        catch (TreeRejectedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new TreeRejectedException(nameof(ExpressionTreeEvaluator), ex);
        }

        if (context.HasRejection)
        {
            throw new TreeRejectedException();
        }

        if (context.Head.CanMoveForward())
        {
            // TODO reevaluated whether this should be tree rejected and not some other ex type
            throw new TreeRejectedException();
        }
    }
}
