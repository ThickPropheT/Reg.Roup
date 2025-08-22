using System.Linq.Expressions;

namespace TreeVal.Condition;

public class WhereCondition : ICondition
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

    public bool Evaluate(Expression node)
    {
        try
        {
            return _predicate(node);
        }
        catch (UnmetPreconditionException upe)
        {
            throw new ConditionFailedException(this, upe);
        }
    }
}
