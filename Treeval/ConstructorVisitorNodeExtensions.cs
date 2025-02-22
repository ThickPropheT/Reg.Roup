namespace Treeval;

public static class ConstructorVisitorNodeExtensions
{
    public static IVisitorNode<System.Linq.Expressions.NewExpression> New(this IVisitorNodeFactory factory)
        => factory
            .OfType<System.Linq.Expressions.NewExpression>()
            .Where(@new => @new.Constructor != null);
}
