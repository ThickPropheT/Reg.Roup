using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public interface IEvaluatorNodeFactory
{
    IEvaluatorNode ToEvaluator();
}

public interface IEvaluatorConditionBuilder : IEvaluatorNodeFactory
{
    void AddCondition(ICondition<Expression> condition);
}

public interface IEvaluatorConditionBuilder<TExpression> : IEvaluatorConditionBuilder
{
}

public interface IEvaluatorBuilder<TExpression> : IEvaluatorConditionBuilder<TExpression>
{
    void AddChildren(Func<TExpression, IEnumerable<IEvaluatorNodeFactory>> getChildren);
}

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
        => VisitationContext.EvaluateTree(expressionTree, _schema);

    public override string? ToString()
        => Name ?? base.ToString();
}
