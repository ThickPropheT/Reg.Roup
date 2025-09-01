using System.Linq.Expressions;

namespace TreeVal.Condition;

public class WhereCondition : ICondition<Expression>
{
    private readonly string _message;
    private readonly Func<Expression, bool> _predicate;

    public WhereCondition(string message, Func<Expression, bool> predicate)
    {
        _message = message;
        _predicate = predicate;
    }

    public void Describe(IDescription description)
        => description.EmitWhereCondition(_message);

    public void Evaluate(Expression node, Evaluation evaluation)
    {
        if (_predicate(node))
            return;

        evaluation.Reject();
    }

    public override string ToString() => _message;
}

public class WhereCondition<TExpression> : ICondition<Expression>
    where TExpression : Expression
{
    private readonly string _message;
    private readonly Func<TExpression, bool> _predicate;

    public WhereCondition(string message, Func<TExpression, bool> predicate)
    {
        _message = message;
        _predicate = predicate;
    }

    public void Describe(IDescription description)
        => description.EmitWhereCondition(_message);

    public void Evaluate(Expression node, Evaluation evaluation)
    {
        if (node is not TExpression t)
        {
            throw new ConditionFailedException(this);
        }

        if (_predicate(t))
            return;

        evaluation.Reject();
    }
}
