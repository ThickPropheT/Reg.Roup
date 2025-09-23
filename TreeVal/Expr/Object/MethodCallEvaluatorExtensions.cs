using System.Linq.Expressions;
using System.Reflection;
using TreeVal.Eval;
using TreeVal.Eval.AcceptChildren;
using TreeVal.Extensions;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Object;

public static class MethodCallEvaluatorExtensions
{
    // this accepts both static & instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall(this IVisitorBuilderFactory factory)
        => factory
            .OfType<MethodCallExpression>()
            .HavingAnyChild();

    // this accepts both static & instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, string? name)
        => factory
            .MethodCallBase(name)
            .HavingAnyChild();

    // this accepts both static, instance, & extension
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, Type ownerType)
        => factory
            .MethodCallBase(ownerType)
            .HavingAnyChild();

    // this accepts both static & instance, but not extension
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(this IVisitorBuilderFactory factory)
        => factory
            .MethodCallBase(typeof(TOwner))
            .HavingAnyChild();

    // this accepts only instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, Func<MethodCallExpression, IVisitorFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Where(call => !call.Method.IsStatic || call.Method.IsExtensionMethod())
            .HavingChild(target)
            .HavingAnyChild();

    // this accepts both static & instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, params IVisitorFactory[] parameters)
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
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, Func<MethodCallExpression, IVisitorBuilder[]> getParameters)
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
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, Type ownerType, string? name)
        => factory
            .MethodCallBase(ownerType, name)
            .HavingAnyChild();

    // this accepts both static & instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorBuilderFactory factory, string? name)
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingAnyChild();

    // this accepts both instance only
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, Type ownerType, Func<MethodCallExpression, IVisitorFactory> target)
        => factory
            .InstanceMethodCallBase(ownerType, target)
            .HavingAnyChild();

    // this accepts both instance only
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorBuilderFactory factory, Func<MethodCallExpression, IVisitorFactory> target)
        => factory
            .InstanceMethodCallBase(typeof(TOwner), target)
            .HavingAnyChild();


    // TODO verify this accepts both static & instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorBuilderFactory factory, params IVisitorFactory[] parameters)
        => factory
            .MethodCallBase(typeof(TOwner))
            .HavingChildren(call =>
                parameters.Length > 0
                    ? parameters
                    : [factory.AcceptChildren(call)]);

    // this accepts only instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall(
        this IVisitorBuilderFactory factory, string? name, Func<MethodCallExpression, IVisitorFactory> target)
        => factory
            .MethodCallBase(name)
            .Where(call => !call.Method.IsStatic || call.Method.IsExtensionMethod())
            .HavingChild(target)
            .HavingAnyChild();

    // TODO verify this accepts both static & instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        params IVisitorFactory[] parameters
    )
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingChildren(call =>
            {
                var target = DefaultMethodCallTarget(factory, call);

                return target.Concat(
                    parameters.Length > 0
                        ? parameters
                        : [factory.AcceptChildren(call)]);
            });

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
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IVisitorBuilder<Expression>> getTarget,
        params IVisitorFactory[] parameters
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
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IVisitorBuilder[]> parameters
    )
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingChildren(parameters);

    // TODO verify this accepts only instance
    public static IVisitorBuilder<MethodCallExpression> MethodCall<TOwner>(
        this IVisitorBuilderFactory factory,
        // TODO
        //  reify this to allow passing in things like `Name.Any()`
        //  add name validation
        string? name,
        Func<MethodCallExpression, IVisitorBuilder<Expression>> getTarget,
        Func<MethodCallExpression, IVisitorBuilder[]> parameters
    )
        => factory
            .MethodCallBase(typeof(TOwner), name)
            .HavingChild(call =>
                getTarget(call)
                    .Equals(typeof(TOwner), @object => @object.Type))
            .HavingChildren(parameters);


    private static IVisitorBuilder<MethodCallExpression> MethodCallBase(
        this IVisitorBuilderFactory factory, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(name, call => call.Method.Name);

    private static IVisitorBuilder<MethodCallExpression> MethodCallBase(
        this IVisitorBuilderFactory factory, Type ownerType)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(ownerType, call => call.Method.DeclaringType);

    private static IVisitorBuilder<MethodCallExpression> MethodCallBase(
        this IVisitorBuilderFactory factory, Type ownerType, string? name)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(ownerType, call => call.Method.DeclaringType)
            .Equals(name, call => call.Method.Name);

    private static IVisitorBuilder<MethodCallExpression> InstanceMethodCallBase(
        this IVisitorBuilderFactory factory, Type ownerType, Func<MethodCallExpression, IVisitorFactory> target)
        => factory
            .OfType<MethodCallExpression>()
            .Equals(ownerType, call => call.Method.DeclaringType)
            .Where(call => !call.Method.IsStatic || call.Method.IsExtensionMethod())
            .HavingChild(target);

    private static IVisitorBuilder[] DefaultMethodCallTarget(
        IVisitorBuilderFactory factory, MethodCallExpression call)
    {
        return !call.Method.IsStatic || call.Method.IsExtensionMethod() 
            ? [factory.AnyMethodTarget(call.Method)] 
            : [];
    }

    public static IVisitorBuilder AnyMethodTarget(
        this IVisitorBuilderFactory factory, MethodInfo method)
    {
        // instance methods
        if (!method.IsStatic)
        {
            return
                factory
                    .AnyOne()
                    // TODO may be yagni since I don't think this is even possible
                    .Is(e => e.Type, method.DeclaringType)
                    .HavingAnyChild();
        }

        // extension methods
        if (method.IsExtensionMethod())
        {
            return
                factory
                    .AnyOne()
                    .With(
                        e => (e, parameters: method.GetParameters()),
                        (node, a) =>
                            node
                                .Where(_ => a.parameters.Length > 0)
                                .Is(_ => a.e.Type, a.parameters[0].ParameterType))
                    .HavingAnyChild();
        }

        // static, non-extension methods
        throw new ArgumentException("Strategy for defining static method targets is indeterminate", nameof(method));
    }
}
