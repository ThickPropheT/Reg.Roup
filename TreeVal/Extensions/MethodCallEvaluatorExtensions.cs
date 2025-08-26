using System.Linq.Expressions;
using System.Reflection;

namespace TreeVal.Extensions;

public static class MethodCallEvaluatorExtensions
{
    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IVisitorNodeFactory factory)
        => factory
            .OfType<MethodCallExpression>()
            .HavingAnyChild();

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IVisitorNodeFactory factory, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.Name, name)
            .HavingAnyChild();

    // this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Where(call => !call.Method.IsStatic)
            .HavingChild(target)
            .HavingAnyChild();

    // TODO verify this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, string? name, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.Name, name)
            .Where(call => !call.Method.IsStatic)
            .HavingChild(target)
            .HavingAnyChild();

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        params IEvaluatorNodeFactory[] parameters)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, typeof(TOwner))
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);

    // TODO verify this accepts both static & instance
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
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);

    // TODO verify this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IEvaluatorConditionBuilder> getTarget,
        params IEvaluatorNodeFactory[] parameters)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, typeof(TOwner))
            .Equals(call => call.Method.Name, name)
            .HavingChild(call =>
                getTarget(call)
                    .Equals(@object => @object.Type, typeof(TOwner)))
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);
    
    // TODO verify this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IEvaluatorConditionBuilder[]> parameters)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, typeof(TOwner))
            .Equals(call => call.Method.Name, name)
            .HavingChildren(parameters);

    // TODO verify this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IEvaluatorConditionBuilder> getTarget,
        Func<MethodCallExpression, IEvaluatorConditionBuilder[]> parameters)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, typeof(TOwner))
            .Equals(call => call.Method.Name, name)
            .HavingChild(call =>
                getTarget(call)
                    .Equals(@object => @object.Type, typeof(TOwner)))
            .HavingChildren(parameters);

    // TODO consider moving the below to their own file
    public static IEvaluatorBuilder MethodCallDelegate(
        this IVisitorNodeFactory factory,
        Func<MethodCallExpression, IEvaluatorNodeFactory>? getTarget = null,
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

                    getTarget?.Invoke(call) ?? factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );

    public static IEvaluatorBuilder MethodCallDelegate(
        this IVisitorNodeFactory factory,
        string? name,
        Func<MethodCallExpression, IEvaluatorNodeFactory>? getTarget = null,
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
                        .Equals(constant => ((MethodInfo) constant.Value!).Name, name)
                        .Where(constant => where?.Invoke((MethodInfo) constant.Value!) != false),

                    factory
                        .OfType<ConstantExpression>()
                        .Equals(constant => constant.Type, typeof(Type)),

                    getTarget?.Invoke(call) ?? factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );
}
