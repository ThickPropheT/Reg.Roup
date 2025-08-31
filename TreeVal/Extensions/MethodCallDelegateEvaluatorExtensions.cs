using System.Linq.Expressions;
using System.Reflection;

namespace TreeVal.Extensions;

public static class MethodCallDelegateEvaluatorExtensions
{
    public static IEvaluatorBuilder<MethodCallExpression> MethodCallDelegate(
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
                        .Equals(typeof(Type), constant => constant.Type),

                    getTarget?.Invoke(call) ?? factory
                        .OfType<ConstantExpression>()
                        .Where(constant => constant.Value == null)
                ]
            );

    public static IEvaluatorBuilder<MethodCallExpression> MethodCallDelegate(
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
