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
    private readonly RootNode _rootNode;

    private ExpressionTreeEvaluator(RootNode rootNode)
    {
        _rootNode = rootNode;
    }

    public static ExpressionTreeEvaluator Create(Func<VisitorNodeFactory, IEvaluatorBuilder> doIt)
    {
        var factory = new VisitorNodeFactory();

        var root = doIt(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

        return new ExpressionTreeEvaluator(new RootNode(root.ToEvaluator()));
    }

    // TODO find a way to return the expression tree here
    public void Evaluate(Expression expressionTree)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(expressionTree).ToArray();
        var head = new TapeHead(tape);

        var visitation = new VisitationContext(head);

        try
        {
            _rootNode.Evaluate(visitation);
        }
        catch (Exception)
        {
            throw new TreeRejectedException();
        }

        if (visitation.HasRejection)
        {
            throw new TreeRejectedException();
        }
    }

    private class RootNode
    {
        private readonly IEvaluatorNode _tree;

        public RootNode(IEvaluatorNode tree)
        {
            _tree = tree;
        }

        public Expression? Evaluate(IVisitationContext context)
        {
            var result = _tree.Evaluate(context);

            if (context.CanMoveForward())
            {
                // TODO reevaluated whether this should be tree rejected and not some other ex type
                throw new TreeRejectedException();
            }

            return result;
        }
    }
}
