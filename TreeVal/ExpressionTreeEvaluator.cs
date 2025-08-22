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
    private readonly IEvaluatorNodeFactory _schema;

    private ExpressionTreeEvaluator(IEvaluatorNodeFactory schema)
    {
        _schema = schema;
    }

    public static ExpressionTreeEvaluator Create(Func<IVisitorNodeFactory, IEvaluatorNodeFactory> buildEvaluatorTree)
    {
        var factory = new VisitorNodeFactory();

        var root = buildEvaluatorTree(factory);

        if (root == null)
        {
            throw new NotSupportedException();
        }

        return new ExpressionTreeEvaluator(root);
    }

    public void Evaluate(Expression expressionTree)
        => VisitationContext.EvaluateTree(expressionTree, _schema);
}
