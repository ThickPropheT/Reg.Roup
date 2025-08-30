using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            .MethodCallBase(name)
            .HavingAnyChild();

    // this accepts both static, instance, & extension
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IVisitorNodeFactory factory, Type ownerType)
        => factory
            .MethodCallBase(ownerType)
            .HavingAnyChild();

    // this accepts both static & instance, but not extension
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(this IVisitorNodeFactory factory)
        => factory
            .MethodCallBase(typeof(TOwner))
            .HavingAnyChild();

    // this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Where(call => !call.Method.IsStatic || IsExtensionMethod(call.Method))
            .HavingChild(target)
            .HavingAnyChild();

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, params IEvaluatorNodeFactory[] parameters)
        => factory
            .OfType<MethodCallExpression>()
            .HavingChildren(call =>
            {
                var target = DefaultMethodCallTarget(factory, call);

                return target.Concat(
                    parameters.Length > 0
                        ? parameters
                        : [factory.AcceptChildren(call)]);
            });

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, Func<MethodCallExpression, IEvaluatorConditionBuilder[]> getParameters)
        => factory
            .OfType<MethodCallExpression>()
            .HavingChildren(call =>
            {
                var target = DefaultMethodCallTarget(factory, call);
                var parameters = getParameters(call);

                return target.Concat(
                    parameters.Length > 0
                        ? parameters
                        : [factory.AcceptChildren(call)]);
            });


    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, Type ownerType, string? name)
        => factory
            .MethodCallBase(ownerType, name)
            .HavingAnyChild();

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory, string? name)
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingAnyChild();

    // this accepts both instance only
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, Type ownerType, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .InstanceMethodCallBase(ownerType, target)
            .HavingAnyChild();

    // this accepts both instance only
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .InstanceMethodCallBase(typeof(TOwner), target)
            .HavingAnyChild();


    // TODO verify this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory, params IEvaluatorNodeFactory[] parameters)
        => factory
            .MethodCallBase(typeof(TOwner))
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);

    // this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IVisitorNodeFactory factory, string? name, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .MethodCallBase(name)
            .Where(call => !call.Method.IsStatic || IsExtensionMethod(call.Method))
            .HavingChild(target)
            .HavingAnyChild();

    // TODO verify this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorNodeFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        params IEvaluatorNodeFactory[] parameters)
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);

    // TODO the params on this were hiding the declaring type, target method above
    // TODO verify this accepts only instance
    // public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
    //     this IVisitorNodeFactory factory,
    //     // TODO
    //     //  reify this to allow passing in things like `Name.Any()`
    //     //  add name validation
    //     Func<MethodCallExpression, IEvaluatorConditionBuilder> getTarget,
    //     params IEvaluatorNodeFactory[] parameters)
    //     => factory
    //         .MethodCallBase(typeof(TOwner))
    //         .HavingChild(call =>
    //             getTarget(call)
    //                 .Equals(@object => @object.Type, typeof(TOwner)))
    //         .HavingChildren(call =>
    //             parameters.Length > 0
    //                 ? parameters
    //                 : [factory.AcceptChildren(call)]);

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
            .MethodCallBase(typeof(TOwner), name)
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
            .MethodCallBase(typeof(TOwner), name)
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
            .MethodCallBase(typeof(TOwner), name)
            .HavingChild(call =>
                getTarget(call)
                    .Equals(@object => @object.Type, typeof(TOwner)))
            .HavingChildren(parameters);


    private static IEvaluatorBuilder<MethodCallExpression> MethodCallBase(
        this IVisitorNodeFactory factory, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.Name, name);

    private static IEvaluatorBuilder<MethodCallExpression> MethodCallBase(
        this IVisitorNodeFactory factory, Type ownerType)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, ownerType);

    private static IEvaluatorBuilder<MethodCallExpression> MethodCallBase(
        this IVisitorNodeFactory factory, Type ownerType, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, ownerType)
            .Equals(call => call.Method.Name, name);

    private static IEvaluatorBuilder<MethodCallExpression> InstanceMethodCallBase(
        this IVisitorNodeFactory factory, Type ownerType, Func<MethodCallExpression, IEvaluatorNodeFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(call => call.Method.DeclaringType, ownerType)
            .Where(call => !call.Method.IsStatic || IsExtensionMethod(call.Method))
            .HavingChild(target);


    private static bool IsExtensionMethod(MethodInfo method)
        => method.IsDefined(typeof(ExtensionAttribute), true);

    private static IEvaluatorBuilder[] DefaultMethodCallTarget(IVisitorNodeFactory factory, MethodCallExpression call)
    {
        if (!call.Method.IsStatic)
        {
            return
            [
                factory
                    .AnyOne()
                    // TODO may be yagni since I don't think this is even possible
                    .Is(e => e.Type, call.Method.DeclaringType)
                    .HavingAnyChild()
            ];
        }

        if (IsExtensionMethod(call.Method))
        {
            // TODO
            //  writing to & reading from this variable that's tantamount to global
            //  may be problematic. keep your wits about you
            ParameterInfo[]? callMethodParameters = null;

            return
            [
                factory
                    .AnyOne()
                    .Where(_ => (callMethodParameters = call.Method.GetParameters()).Length > 0)
                    .Is(e => e.Type, () => callMethodParameters?[0].ParameterType)
                    .HavingAnyChild()
            ];
        }

        return [];
    }
}
