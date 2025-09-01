using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public interface IDescription
{
    void EmitResult(Evaluation.Status status, IEvaluatorNode evaluator, ICondition<Expression>[] failedConditions);
    void EmitNodeTypeCondition(ExpressionType nodeType);
    void EmitNodeTypeCondition(Type type, ExpressionType? nodeType = null);
    void EmitWhereCondition(string message);
}

public interface IDescribable
{
    // TODO
    //  - add `DescriptionContext` param w/ members:
    //    - Emit(string header, string? value = null)
    //    - Visit(IDescribable child)
    //    - Visit(IEnumerable<IDescribable> children)
    void Describe(IDescription description);
}
