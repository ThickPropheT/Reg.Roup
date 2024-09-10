using System;
using System.Linq.Expressions;

namespace Reg.Roup.Conversions
{
    public class SupportedConversions
    {
        public static readonly Type ParseType = typeof(IParse);

        public static void Validate(MethodCallExpression expression)
        {
            var method = expression.Method;

            if (method.DeclaringType != ParseType)
            {
                throw new NotSupportedException(
                    $"Expression may only contain calls to methods belonging to type [{ParseType}], but encountered method [{method}]."
                );
            }
        }

        public static void Validate(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                Validate(methodCallExpression);
            }
            else if (expression is not ConstantExpression)
            {
                throw new NotSupportedException(
                    $"Expression may only contain calls to methods belonging to type [{ParseType}], conversion operations, boxing operations, and constant assignment operations, but encountered [{expression}]."
                );
            }
        }
    }
}
