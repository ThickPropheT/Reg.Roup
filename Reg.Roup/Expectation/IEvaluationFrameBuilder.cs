using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public interface IEvaluationFrameBuilder
{
    IEvaluationFrame BuildFrame(Expression? node);
}