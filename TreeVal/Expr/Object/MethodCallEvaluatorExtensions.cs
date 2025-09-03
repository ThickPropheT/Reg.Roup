using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using TreeVal.Eval;
using TreeVal.Eval.AcceptChildren;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Object;

public static class MethodCallEvaluatorExtensions
{
    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IEvaluatorBuilderFactory factory)
        => factory
            .OfType<MethodCallExpression>()
            .HavingAnyChild();

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IEvaluatorBuilderFactory factory, string? name)
        => factory
            .MethodCallBase(name)
            .HavingAnyChild();

    // this accepts both static, instance, & extension
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(this IEvaluatorBuilderFactory factory, Type ownerType)
        => factory
            .MethodCallBase(ownerType)
            .HavingAnyChild();

    // this accepts both static & instance, but not extension
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(this IEvaluatorBuilderFactory factory)
        => factory
            .MethodCallBase(typeof(TOwner))
            .HavingAnyChild();

    // this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IEvaluatorBuilderFactory factory, Func<MethodCallExpression, INodeEvaluatorFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Where(call => !call.Method.IsStatic || IsExtensionMethod(call.Method))
            .HavingChild(target)
            .HavingAnyChild();

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IEvaluatorBuilderFactory factory, params INodeEvaluatorFactory[] parameters)
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
        this IEvaluatorBuilderFactory factory, Func<MethodCallExpression, IEvaluatorBuilder[]> getParameters)
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
        this IEvaluatorBuilderFactory factory, Type ownerType, string? name)
        => factory
            .MethodCallBase(ownerType, name)
            .HavingAnyChild();

    // this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IEvaluatorBuilderFactory factory, string? name)
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingAnyChild();

    // this accepts both instance only
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IEvaluatorBuilderFactory factory, Type ownerType, Func<MethodCallExpression, INodeEvaluatorFactory> target)
        => factory
            .InstanceMethodCallBase(ownerType, target)
            .HavingAnyChild();

    // this accepts both instance only
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IEvaluatorBuilderFactory factory, Func<MethodCallExpression, INodeEvaluatorFactory> target)
        => factory
            .InstanceMethodCallBase(typeof(TOwner), target)
            .HavingAnyChild();


    // TODO verify this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IEvaluatorBuilderFactory factory, params INodeEvaluatorFactory[] parameters)
        => factory
            .MethodCallBase(typeof(TOwner))
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);

    // this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall(
        this IEvaluatorBuilderFactory factory, string? name, Func<MethodCallExpression, INodeEvaluatorFactory> target)
        => factory
            .MethodCallBase(name)
            .Where(call => !call.Method.IsStatic || IsExtensionMethod(call.Method))
            .HavingChild(target)
            .HavingAnyChild();

    // TODO verify this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IEvaluatorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        params INodeEvaluatorFactory[] parameters
    )
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
        this IEvaluatorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IEvaluatorBuilder<Expression>> getTarget,
        params INodeEvaluatorFactory[] parameters
    )
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingChild(call =>
                getTarget(call)
                    .Equals(typeof(TOwner), @object => @object.Type))
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);

    // TODO verify this accepts both static & instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IEvaluatorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IEvaluatorBuilder[]> parameters
    )
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingChildren(parameters);

    // TODO verify this accepts only instance
    public static IEvaluatorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IEvaluatorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IEvaluatorBuilder<Expression>> getTarget,
        Func<MethodCallExpression, IEvaluatorBuilder[]> parameters
    )
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingChild(call =>
                getTarget(call)
                    .Equals(typeof(TOwner), @object => @object.Type))
            .HavingChildren(parameters);


    private static IEvaluatorBuilder<MethodCallExpression> MethodCallBase(
        this IEvaluatorBuilderFactory factory, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(name, call => call.Method.Name);

    private static IEvaluatorBuilder<MethodCallExpression> MethodCallBase(
        this IEvaluatorBuilderFactory factory, Type ownerType)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(ownerType, call => call.Method.DeclaringType);

    private static IEvaluatorBuilder<MethodCallExpression> MethodCallBase(
        this IEvaluatorBuilderFactory factory, Type ownerType, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(ownerType, call => call.Method.DeclaringType)
            .Equals(name, call => call.Method.Name);

    private static IEvaluatorBuilder<MethodCallExpression> InstanceMethodCallBase(
        this IEvaluatorBuilderFactory factory, Type ownerType, Func<MethodCallExpression, INodeEvaluatorFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(ownerType, call => call.Method.DeclaringType)
            .Where(call => !call.Method.IsStatic || IsExtensionMethod(call.Method))
            .HavingChild(target);


    private static bool IsExtensionMethod(MethodInfo method)
        => method.IsDefined(typeof(ExtensionAttribute), true);

    private static IEvaluatorBuilder[] DefaultMethodCallTarget(IEvaluatorBuilderFactory factory, MethodCallExpression call)
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
            return
            [
                factory
                    .AnyOne()
                    .With(
                        e => (e, parameters: call.Method.GetParameters()),
                        (node, a) =>
                            node
                                .Where(_ => a.parameters.Length > 0)
                                .Is(_ => a.e.Type, a.parameters[0].ParameterType))
                    .HavingAnyChild()
            ];
        }

        return [];
    }
}
