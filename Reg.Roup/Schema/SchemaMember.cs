using System;
using Reg.Roup.Utility;
using Reg.Roup.Conversions;
using System.Text.RegularExpressions;

namespace Reg.Roup.Schema
{
    using Reg.Roup.Expression;
    using System.Linq.Expressions;

    public class SchemaMember
    {
        private readonly Expression _expression;

        public string Name { get; }
        public Type Type { get; }
        protected bool IsOptional { get; }

        public SchemaMember(string name, Type type, Expression expression)
        {
            Name = name;
            Type = type;
            IsOptional = type.CanBeAssignedNull();

            _expression = IgnoreImplicitConversions(expression);
        }

        private Expression IgnoreImplicitConversions(Expression expression)
        {
            if (expression is UnaryExpression conversionExpression)
            {
                // TODO there's a chance this is actually a valid case. keep your eyes peeled.
                if (!IsOptional)
                {
                    throw new InvalidCastException(
                        $"Suspicious implicit conversion or boxing operation for non-nullable target with name '{Name}' of type [{Type}]."
                    );
                }

                return conversionExpression.Operand;
            }

            return expression;
        }

        public Func<string?, object?>? FindConverter()
        {
            if (_expression is not MethodCallExpression methodCall)
                return null;

            return value =>
            {
                var argumentExpression = methodCall.Arguments[0];
                var parseDelegate = (Delegate)Expression.Lambda(argumentExpression).Compile().DynamicInvoke()!;

                return parseDelegate.DynamicInvoke(value);
            };
        }

        public bool IsValid(Regex regex)
            => IsOptional
            || regex.GroupNumberFromName(Name) >= 0;

        public IExpression<string?> CreateReaderOf(GroupCollection groups)
        {
            var candidate = groups[Name];

            if (candidate?.Success == true)
            {
                return new GroupReader(this, candidate);
            }
            if (!IsOptional)
            {
                throw new FormatException(
                    $"Regex could not match group '{Name}' for input string, but group is not optional (nullable)."
                );
            }

            return new Constant(this, null);
        }

        public object? Convert(string? value)
        {
            if (string.IsNullOrEmpty(value) && IsOptional)
            {
                return null;
            }

            return System.Convert.ChangeType(value, Type.TryGetNullableType() ?? Type);
        }
    }
}
