using System.Linq.Expressions;

namespace Treeval.Condition;

public interface ICondition : IDescribable
{
    bool Evaluate(Expression? node);
}
