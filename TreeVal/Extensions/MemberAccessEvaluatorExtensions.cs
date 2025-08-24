using System.Linq.Expressions;
using System.Reflection;

namespace TreeVal.Extensions;

public static class MemberAccessEvaluatorExtensions
{
    public static IEvaluatorBuilder<MemberExpression> ReadProperty<TOwner>(this IVisitorNodeFactory factory)
        => factory
            .OfType<MemberExpression>()
            .Equals(member => member.Type, typeof(TOwner))
            .Where(member => member.Member is PropertyInfo);

    public static IEvaluatorBuilder<MemberExpression> ReadProperty<TOwner>(this IVisitorNodeFactory factory,
        string? name)
        => factory
            .OfType<MemberExpression>()
            .Equals(member => member.Type, typeof(TOwner))
            .Where(member => member.Member is PropertyInfo)
            .Equals(member => member.Member.Name, name);

    public static IEvaluatorBuilder<MemberExpression> ReadProperty<TOwner>(this IVisitorNodeFactory factory,
        Expression<Func<TOwner, object>> property)
        => factory
            .OfType<MemberExpression>()
            .Equals(member => member.Type, typeof(TOwner))
            .Equals(member => member.Member.GetType(), typeof(PropertyInfo))
            .Where(member => property.Body is MemberExpression m && member.Member.Name == m.Member.Name);
}
