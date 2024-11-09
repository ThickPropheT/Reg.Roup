namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public interface IExpectationEvaluator
    {
        EvaluationResult Evaluate(Expression? node);
    }
}