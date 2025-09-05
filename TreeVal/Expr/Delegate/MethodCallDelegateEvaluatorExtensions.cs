using System.Linq.Expressions;
using System.Reflection;
using TreeVal.Eval;
using TreeVal.Expr.Conversion;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Delegate;

public static class MethodCallDelegateEvaluatorExtensions
{
    public static IEvaluatorBuilder MethodCallDelegate<TDelegate>(
        this IEvaluatorBuilderFactory factory,
        Func<MethodCallExpression, INodeEvaluatorFactory>? getTarget = null,
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
                    // TODO
                    //  had this arrangement originally, but it was failing for parse.With(Parser.Instance.Parse)
                    //  b/c the order of these 2 was reversed. i bet this is order correct in some other use case.
                    // factory
                    //     .OfType<ConstantExpression>()
                    //     .Debug(
                    //         label: "After OfType in MethodCallDelegate",
                    //         (o, ctx) =>
                    //         {
                    //             
                    //         })
                    //     .Where(constant => constant.Value is MethodInfo)
                    //     .When(where).IsNotNull()
                    //     // ReSharper disable once VariableHidesOuterVariable
                    //     .Then((node, where) =>
                    //         node.Equals(true, constant => where((MethodInfo) constant.Value!))),
                    //
                    // factory
                    //     .OfType<ConstantExpression>()
                    //     .Equals(typeof(Type), constant => constant.Type),

                    // MethodInfo for the method to convert to Delegate
                    factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value is MethodInfo)
                        .When(where).IsNotNull()
                        // ReSharper disable once VariableHidesOuterVariable
                        .Then((node, where) =>
                            node.Equals(true, constant => where((MethodInfo) constant.Value!))),

                    // Type of Delegate to convert the method to
                    factory
                        .OfType<ConstantExpression>()
                        .Equals(typeof(Type), constant => constant.Type)
                        .Equals(typeof(TDelegate), constant => constant.Value),

                    getTarget?.Invoke(call) ?? factory
                        // TODO i think this part may actually be targeted at static methods
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );

    public static IEvaluatorBuilder MethodCallDelegate(
        this IEvaluatorBuilderFactory factory,
        string? name,
        Func<MethodCallExpression, INodeEvaluatorFactory>? getTarget = null,
        Func<MethodInfo, bool>? where = null
    )
        => factory
            .IgnoreBoxing()
            .OfType<MethodCallExpression>()
            .Where(call => call.Method.DeclaringType == typeof(MethodInfo))
            .Where(call => call.Method.Name == nameof(MethodInfo.CreateDelegate))
            .HavingChildren(call =>
                [
                    factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value is MethodInfo)
                        .Equals(name, constant => ((MethodInfo) constant.Value!).Name)
                        .Where(constant => where?.Invoke((MethodInfo) constant.Value!) != false),

                    factory
                        .OfType<ConstantExpression>()
                        .Equals(typeof(Type), constant => constant.Type),

                    getTarget?.Invoke(call) ?? factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );
}
