using System.Linq.Expressions;

namespace TreeVal.Condition;

public interface ICondition : IDescribable
{
    bool Evaluate(Expression? node);
}
