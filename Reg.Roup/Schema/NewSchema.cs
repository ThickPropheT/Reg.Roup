using Reg.Roup.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reg.Roup.Schema
{
    public class NewSchema
    {
        private readonly ConstructorInfo _constructor;

        public SchemaMember[] Parameters { get; }

        protected NewSchema(ConstructorInfo constructor, SchemaMember[] parameters)
        {
            _constructor = constructor;
            Parameters = parameters;
        }

        public static NewSchema From(NewExpression schema)
        {
            if (schema.Constructor == null
                || !schema.Arguments.Any())
            {
                throw new NotSupportedException(
                    "Expected either parameterized constructor or object initializer."
                );
            }

            var constructor = schema.Constructor;

            return new NewSchema(
                constructor,
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

                    // TODO
                    //  assuming that these are in the correct order (arguments)
                    //  test named & optional parameters
                    return new SchemaMember(p.Name, p.ParameterType, arguments[i]);
                })
                .ToArray();

        public virtual object CreateInstanceFrom(MatchContext match)
        {
            var args = Parameters
                .Select(m => Conversion.Extract(m).From(match))
                .Select(c => c.Apply())
                .ToArray();

            return _constructor.Invoke(args);
        }
    }
}
