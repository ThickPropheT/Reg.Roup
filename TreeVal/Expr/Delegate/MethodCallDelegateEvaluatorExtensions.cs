using System.Linq.Expressions;
using System.Reflection;
using TreeVal.Expr.Constant;
using TreeVal.Expr.Conversion;
using TreeVal.Expr.Object;
using TreeVal.Extensions;
using TreeVal.Primitives;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Case;
using TreeVal.Stage.Children.HavingChildren;
using TreeVal.Stage.Eval.Equals;
using TreeVal.Stage.Eval.Where;
using TreeVal.Visit.With;

namespace TreeVal.Expr.Delegate;

public static class MethodCallDelegateEvaluatorExtensions
{
    public static IVisitorBuilder MethodCallDelegate<TDelegate>(
        this IVisitorBuilderFactory factory,
        Func<MethodCallExpression, IVisitorBuilder>? getTarget = null,
        Func<MethodInfo, bool>? where = null
    )
        where TDelegate : System.Delegate
        => factory
            .IgnoreBoxing()
            .OfType<MethodCallExpression>()
            .Where(call => call.Method.DeclaringType == typeof(MethodInfo))
            .Where(call => call.Method.Name == nameof(MethodInfo.CreateDelegate))
            .HavingChildren(call =>
            [
                factory
                    .OfType<ConstantExpression>()
                    .With(
                        constant => (constant, methodInfo: constant.Value as MethodInfo),
                        (builder, a) =>
                            builder
                                .Where(_ => a.methodInfo != null)

                                // arg[0]: delegateType
                                .HavingChild(_ => DelegateType<TDelegate>(factory))
                                .Case((@case, _) =>
                                [
                                    @case.When(where)
                                        .IsNotNull()
                                        .Then((node, predicate) => node.Equals(true, _ => predicate(a.methodInfo!))),

                                    // arg[1]: target

                                    // instance & extension methods
                                    @case.When(a.methodInfo)
                                        .IsTrue(methodInfo => !methodInfo.IsStatic || methodInfo.IsExtensionMethod())
                                        .Then((node, _) =>
                                            node.HavingChild(_ =>
                                                getTarget?.Invoke(call)
                                                ?? factory.AnyMethodTarget(a.methodInfo!))),

                                    // static, non-extension methods
                                    @case.When(a.methodInfo)
                                        .IsTrue(methodInfo => methodInfo.IsStatic && !methodInfo.IsExtensionMethod())
                                        .Then((node, _) =>
                                            node.HavingChild(_ =>
                                                factory.Constant(EValue.Null()))),
                                ])
                    )
            ]);

    private static IVisitorBuilder DelegateType<TDelegate>(IVisitorBuilderFactory factory)
        => factory
            .OfType<ConstantExpression>()
            .Equals(typeof(Type), constant => constant.Type)
            .Equals(typeof(TDelegate), constant => constant.Value);

    // TODO
    // public static IEvaluatorBuilder MethodCallDelegate(
    //     this IEvaluatorBuilderFactory factory,
    //     string? name,
    //     Func<MethodCallExpression, INodeEvaluatorFactory>? getTarget = null,
    //     Func<MethodInfo, bool>? where = null
    // )
    //     => factory
    //         .IgnoreBoxing()
    //         .OfType<MethodCallExpression>()
    //         .Where(call => call.Method.DeclaringType == typeof(MethodInfo))
    //         .Where(call => call.Method.Name == nameof(MethodInfo.CreateDelegate))
    //         .HavingChildren(call =>
    //             [
    //                 factory
    //                     .OfType<ConstantExpression>()
    //                     .Where(constant => constant.Value is MethodInfo)
    //                     .Equals(name, constant => ((MethodInfo) constant.Value!).Name)
    //                     .Where(constant => where?.Invoke((MethodInfo) constant.Value!) != false),
    //
    //                 factory
    //                     .OfType<ConstantExpression>()
    //                     .Equals(typeof(Type), constant => constant.Type),
    //
    //                 getTarget?.Invoke(call) ?? factory
    //                     .OfType<ConstantExpression>()
    //                     .Where(constant => constant.Value == null)
    //             ]
    //         );
}
