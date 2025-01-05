namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public interface IEvaluationFrameBuilder
    {
        IEvaluationFrame BuildFrame(Expression? node);
    }
}