using System.Linq.Expressions;
using System.Reflection;

namespace TreeVal.Extensions;

public static class MemberAccessEvaluatorExtensions
{
    public static IEvaluatorBuilder<MemberExpression> ReadProperty<TOwner>(this IVisitorNodeFactory factory)
        => factory
            .OfType<MemberExpression>()
            .Equals(typeof(TOwner), member => member.Type)
            .Where(member => member.Member is PropertyInfo);

    public static IEvaluatorBuilder<MemberExpression> ReadProperty<TOwner>(
        this IVisitorNodeFactory factory, string? name)
        => factory
            .OfType<MemberExpression>()
            .Equals(typeof(TOwner), member => member.Type)
            .Where(member => member.Member is PropertyInfo)
            .Equals(name, member => member.Member.Name);

    public static IEvaluatorBuilder<MemberExpression> ReadProperty<TOwner>(
        this IVisitorNodeFactory factory, Expression<Func<TOwner, object>> property)
        => factory
            .OfType<MemberExpression>()
            .Equals(typeof(TOwner), member => member.Type)
            .Equals(typeof(PropertyInfo), member => member.Member.GetType())
            .Where(member => property.Body is MemberExpression m && member.Member.Name == m.Member.Name);
}
