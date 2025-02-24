using System.Linq.Expressions;
using System.Reflection;

namespace TreeVal;

public static class MethodCallVisitorNodeExtensions
{
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IVisitorNodeFactory factory, Func<MethodCallExpression, IEvaluatorBuilder> target)
        => factory
            .OfType<MethodCallExpression>()
            .HavingChildren(call => [target(call)]);
    
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        params IEvaluatorBuilder[] parameters)
        => factory
            .OfType<MethodCallExpression>()
            .Where(call => call.Method.DeclaringType == typeof(TOwner))
            .Where(call => name == null || call.Method.Name == name)
            .HavingChildren(
                new[]
                    {
                        factory
                            .OfType<Expression>()
                            .Where(@object => @object.Type == typeof(TOwner))
                    }
                    .Concat(parameters)
                    .ToArray());

    public static IEvaluatorBuilder MethodCallDelegate(
        this IVisitorNodeFactory factory, Func<MethodCallExpression, IEvaluatorBuilder>? target = null,
        Func<MethodInfo, bool>? methodPredicate = null)
        => factory
            .OfType<MethodCallExpression>()
            .Where(call => call.Method.DeclaringType == typeof(MethodInfo))
            .Where(call => call.Method.Name == nameof(MethodInfo.CreateDelegate))
            .HavingChildren(call =>
                [
                    factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value is MethodInfo)
                        .Where(constant => methodPredicate?.Invoke((MethodInfo) constant.Value!) != false),

                    factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Type == typeof(Type)),

                    target?.Invoke(call) ?? factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );
}
