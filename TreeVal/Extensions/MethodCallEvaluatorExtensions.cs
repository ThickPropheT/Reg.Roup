using System.Linq.Expressions;
using System.Reflection;

namespace TreeVal.Extensions;

public static class MethodCallEvaluatorExtensions
{
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IVisitorNodeFactory factory)
        => factory
            .OfType<MethodCallExpression>()
            .HavingChild(factory.AcceptChildren);
    
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IVisitorNodeFactory factory, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(method => method.Method.Name, name)
            .HavingChild(factory.AcceptChildren);

    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .HavingChildren(call => [target(call)]);

    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        params IEvaluatorNodeFactory[] parameters)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, typeof(TOwner))
            .HavingChildren(
                new[]
                    {
                        factory.Equals(@object => @object.Type, typeof(TOwner))
                    }
                    .Concat(parameters)
                    .ToArray());

    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        params IEvaluatorNodeFactory[] parameters)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, typeof(TOwner))
            .Equals(call => call.Method.Name, name)
            .HavingChildren(
                new[]
                    {
                        factory.Equals(@object => @object.Type, typeof(TOwner))
                    }
                    .Concat(parameters)
                    .ToArray());
    
    public static IEvaluatorBuilder MethodCallDelegate(
        this IVisitorNodeFactory factory,
        Func<MethodCallExpression, IEvaluatorNodeFactory>? target = null,
        Func<MethodInfo, bool>? where = null)
        => factory
            .IgnoreBoxing(hint: BoxingEvaluationHint.Eager)
            .OfType<MethodCallExpression>()
            .Where(call => call.Method.DeclaringType == typeof(MethodInfo))
            .Where(call => call.Method.Name == nameof(MethodInfo.CreateDelegate))
            .HavingChildren(call =>
                [
                    factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value is MethodInfo)
                        .Where(constant => where?.Invoke((MethodInfo) constant.Value!) != false),

                    factory
                        .OfType<ConstantExpression>()
                        .Equals(constant => constant.Type, typeof(Type)),

                    target?.Invoke(call) ?? factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );

    public static IEvaluatorBuilder MethodCallDelegate(
        this IVisitorNodeFactory factory,
        string? name,
        Func<MethodCallExpression, IEvaluatorNodeFactory>? target = null,
        Func<MethodInfo, bool>? where = null)
        => factory
            .IgnoreBoxing(hint: BoxingEvaluationHint.Eager)
            .OfType<MethodCallExpression>()
            .Where(call => call.Method.DeclaringType == typeof(MethodInfo))
            .Where(call => call.Method.Name == nameof(MethodInfo.CreateDelegate))
            .HavingChildren(call =>
                [
                    factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value is MethodInfo)
                        .Equals(constant => ((MethodInfo)constant.Value!).Name, name)
                        .Where(constant => where?.Invoke((MethodInfo) constant.Value!) != false),

                    factory
                        .OfType<ConstantExpression>()
                        .Equals(constant => constant.Type, typeof(Type)),

                    target?.Invoke(call) ?? factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );
}
