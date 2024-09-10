using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Reg.Roup.Schema
{
    public class SettableSchemaMember : SchemaMember
    {
        private readonly Action<object, object?> _setMember;

        private SettableSchemaMember(string name, Type type, Expression expression, Action<object, object?> setMember)
            : base(name, type, expression)
        {
            _setMember = setMember;
        }

        public static SettableSchemaMember From(MemberInfo member, Expression expression)
        {
            if (member is FieldInfo field)
                return new SettableSchemaMember(field.Name, field.FieldType, expression, field.SetValue);

            else if (member is PropertyInfo property)
                return new SettableSchemaMember(property.Name, property.PropertyType, expression, property.SetValue);

            throw new NotSupportedException(
                $"Expected expression to contain only field and property initializers, but encountered member with name [{member.Name}]."
            );
        }

        public void Set(object obj, object? value)
            => _setMember(obj, value);
    }
}