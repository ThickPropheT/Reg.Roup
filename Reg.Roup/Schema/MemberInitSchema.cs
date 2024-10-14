using Reg.Roup.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Reg.Roup.Schema
{
    public class MemberInitSchema : ConstructorInitSchema
    {
        private readonly SettableSchemaMember[] _initializers;

        private MemberInitSchema(CreateInstance createInstance, SchemaMember[] parameters, SettableSchemaMember[] initializers)
            : base(createInstance, parameters)
        {
            _initializers = initializers;
        }

        public static MemberInitSchema From(MemberInitExpression schema)
        {
            var newExpression = schema.NewExpression;

            var constructor = newExpression.Constructor
                ?? newExpression.Type.GetConstructor(Type.EmptyTypes);

            return new MemberInitSchema(
                constructor != null
                    ? constructor.Invoke
                    // Activator only returns null for nullable & those can't be created w/ member initializers
                    : _ => Activator.CreateInstance(newExpression.Type)!,
                constructor != null
                    ? ExtractParameters(constructor, newExpression.Arguments)
                    : [],
                ExtractInitializers(schema.Bindings)
            );
        }

        private static SettableSchemaMember[] ExtractInitializers(IEnumerable<MemberBinding> bindings)
            => bindings.Select(b =>
            {
                if (b is not MemberAssignment memberAssignment)
                {
                    throw new NotSupportedException(
                        "Neither recursive nor collection member initialization are supported."
                    );
                }

                return SettableSchemaMember.From(b.Member, memberAssignment.Expression);
            })
            .ToArray();

        public override object CreateInstanceFrom(MatchContext match)
        {
            var instance = base.CreateInstanceFrom(match);

            foreach (var (member, value) in _initializers
                .Select(i => (member: i, conversion: Conversion.Extract(i).From(match)))
                .Select(a => (a.member, value: a.conversion.Apply())))
            {
                member.Set(instance, value);
            }

            return instance;
        }
    }
}
