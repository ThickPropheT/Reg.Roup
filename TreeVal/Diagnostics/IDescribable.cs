using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Eval.Condition;

namespace TreeVal.Diagnostics;

public interface IDescription
{
    void EmitResult(Evaluation.Status status, INodeEvaluator evaluator, ICondition[] failedConditions);
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
