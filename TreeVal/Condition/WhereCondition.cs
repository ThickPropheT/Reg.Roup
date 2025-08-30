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

    public void Evaluate(Expression node, Evaluation evaluation)
    {
        try
        {
            if (!_predicate(node)) 
                evaluation.Reject();
        }
        catch (UnmetPreconditionException upe)
        {
            throw new ConditionFailedException(this, upe);
        }
    }

    public override string ToString() => _message;
}
