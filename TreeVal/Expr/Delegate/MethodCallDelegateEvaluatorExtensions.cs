using System.Linq.Expressions;
using System.Reflection;
using TreeVal.Eval;
using TreeVal.Expr.Conversion;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Delegate;

public static class MethodCallDelegateEvaluatorExtensions
{
    public static IEvaluatorBuilder MethodCallDelegate(
        this IEvaluatorBuilderFactory factory,
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
                        .Where(constant => where?.Invoke((MethodInfo) constant.Value!) != false),

                    factory
                        .OfType<ConstantExpression>()
                        .Equals(typeof(Type), constant => constant.Type),

                    getTarget?.Invoke(call) ?? factory
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
