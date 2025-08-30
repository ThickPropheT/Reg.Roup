using System.Linq.Expressions;

namespace TreeVal.Condition;

public interface ICondition : IDescribable
{
    void Evaluate(Expression node, Evaluation evaluation);
}
