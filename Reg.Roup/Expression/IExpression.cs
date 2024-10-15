namespace Reg.Roup.Expression
{
    public interface IExpression<TResult>
    {
        TResult Evaluate();
    }

    public interface IExpression<TArg, TResult>
    {
        TResult Evaluate(TArg arg);
    }
}
