using Reg.Roup.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Reg.Roup._Expression;

namespace Reg.Roup.Schema
{
    using System.Linq.Expressions;

    public delegate object CreateInstance(object?[]? args);

    public class ConstructorInitSchema : IExpression<MatchContext, object>
    {
        private readonly CreateInstance _createInstance;

        public SchemaMember[] Parameters { get; }

        protected ConstructorInitSchema(CreateInstance createInstance, SchemaMember[] parameters)
        {
            _createInstance = createInstance;
            Parameters = parameters;
        }

        public static ConstructorInitSchema From(NewExpression schema)
        {
            if (schema.Constructor == null
                || schema.Arguments.Count == 0)
            {
                throw new NotSupportedException(
                    "Expected either parameterized constructor or object initializer."
                );
            }

            var constructor = schema.Constructor;

            return new ConstructorInitSchema(
                constructor.Invoke,
                ExtractParameters(constructor, schema.Arguments)
            );
        }

        protected static SchemaMember[] ExtractParameters(ConstructorInfo constructor, IList<Expression> arguments)
            => constructor
                .GetParameters()
                .Select((p, i) =>
                {
                    if (p.Name == null)
                    {
                        throw new NotSupportedException(
                            $"Expected constructor parameter at index {i} of type [{p.ParameterType}] to have a name."
                        );
                    }

                    // this safely assumes that arguments are in the correct order.
                    // the only way to mismatch the order would be via named & optional parameters,
                    // which are not supported in expression trees.
                    return new SchemaMember(p.Name, p.ParameterType, arguments[i]);
                })
                .ToArray();

        public virtual object Evaluate(MatchContext match)
        {
            var args = Parameters
                .Select(m => Conversion.Extract(m).From(match))
                .Select(c => c.Evaluate())
                .ToArray();

            return _createInstance.Invoke(args);
        }
    }
}
