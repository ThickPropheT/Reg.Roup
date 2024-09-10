using Reg.Roup.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reg.Roup.Schema
{
    public class MemberInitSchema : NewSchema
    {
        private readonly SettableSchemaMember[] _initializers;

        private MemberInitSchema(ConstructorInfo constructor, SchemaMember[] parameters, SettableSchemaMember[] initializers)
            : base(constructor, parameters)
        {
            _initializers = initializers;
        }

        public static MemberInitSchema From(MemberInitExpression schema)
        {
            var newExpression = schema.NewExpression;
            var constructor = newExpression.Constructor;

            return new MemberInitSchema(
                constructor
                    // TODO hope this works
                    ?? newExpression.Type.GetConstructor(Type.EmptyTypes)
                    // TODO
                    ?? throw new Exception("RUH ROH"),
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

            foreach (var a in _initializers
                // TODO why can't select figure out anonymous structs i.e. new(i: 0, s: '')
                .Select(i => new { member = i, conversion = Conversion.Extract(i).From(match) })
                .Select(a => new { a.member, value = a.conversion.Apply() }))
            {
                a.member.Set(instance, a.value);
            }

            return instance;
        }
    }
}
