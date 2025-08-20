using System.Linq.Expressions;

namespace TreeVal.Condition;

// TODO
//  is this still useful if we operate under the assumption that
//  the linear recorder will never produce null expressions?
public class NotNullCondition : ICondition
{
    public void Describe(IDescription description)
    {}

    public bool Evaluate(Expression? node)
        => node is not null;
}
