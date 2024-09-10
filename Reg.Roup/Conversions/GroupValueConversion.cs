using System;
using System.ComponentModel;

namespace Reg.Roup.Conversions
{
    public class GroupValueConversion
    {
        private readonly GroupValue _value;
        private readonly Func<string?, object?> _convert;

        private GroupValueConversion(GroupValue value, Func<string?, object?> convert)
        {
            _value = value;
            _convert = convert;
        }

        public static GroupValueConversion Explicit(GroupValue value, Func<string?, object?> convert)
            => new(value, convert);

        public static GroupValueConversion Implicit(GroupValue value)
            => new(value, v => TypeDescriptor.GetConverter(value.Member.Type).ConvertTo(v, value.Member.Type));

        public static GroupValueConversion None(GroupValue value)
            => new(value, v => v);

        public object? Apply()
            => _convert(_value.Get());
    }
}
