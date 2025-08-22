using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace TreeVal.Condition;

public class UnmetPreconditionException : Exception
{
    private UnmetPreconditionException(string message)
        : base(message)
    {
    }
    
    // e is T
    // t1 is t2
    // t2.IsAssignableFrom(t1)
    // t1.IsAssignableTo(t2)
    public static UnmetPreconditionException WrongExpressionType<T>(Expression e)
        => new ($"Expected expression node type to be assignable to {typeof(T).FullName}, but was {e.GetType().FullName}.");
}
