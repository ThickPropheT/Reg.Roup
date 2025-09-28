using System.Linq.Expressions;
using System.Reflection;
using TreeVal.Scaffolding;
using TreeVal.Stage.Eval.Equals;
using TreeVal.Stage.Eval.Where;

namespace TreeVal.Expr.Object;

public static class MemberAccessEvaluatorExtensions
{
    public static IVisitorBuilder<MemberExpression> ReadProperty<TOwner>(this IVisitorBuilderFactory factory)
        => factory
            .OfType<MemberExpression>()
            .Equals(typeof(TOwner), member => member.Type)
            .Where(member => member.Member is PropertyInfo);

    public static IVisitorBuilder<MemberExpression> ReadProperty<TOwner>(
        this IVisitorBuilderFactory factory, string? name)
        => factory
            .OfType<MemberExpression>()
            .Equals(typeof(TOwner), member => member.Type)
            .Where(member => member.Member is PropertyInfo)
            .Equals(name, member => member.Member.Name);

    public static IVisitorBuilder<MemberExpression> ReadProperty<TOwner>(
        this IVisitorBuilderFactory factory, Expression<Func<TOwner, object>> property)
        => factory
            .OfType<MemberExpression>()
            .Equals(typeof(TOwner), member => member.Type)
            .Equals(typeof(PropertyInfo), member => member.Member.GetType())
            .Where(member => property.Body is MemberExpression m && member.Member.Name == m.Member.Name);
}
