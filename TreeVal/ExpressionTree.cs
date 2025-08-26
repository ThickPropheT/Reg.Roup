using System.Linq.Expressions;

namespace TreeVal;

public static class ExpressionTree
{
    public static Expression FromBody(Expression<Action> expression) 
        => expression.Body;
    
    public static Expression FromBody<TResult>(Expression<Func<TResult>> expression) 
        => expression.Body;
}
