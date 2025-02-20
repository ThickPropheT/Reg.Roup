using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public interface IEvaluationFrameBuilder : IDescribable
{
    IEvaluationFrame BuildFrame(Expression? node);
}
