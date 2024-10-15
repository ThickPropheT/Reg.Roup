using Reg.Roup.Expression;
using Reg.Roup.Schema;
using System;

namespace Reg.Roup.Conversions
{
    public class ValueConversion : IExpression<object?>
    {
        private readonly IExpression<string?> _reader;
        private readonly Func<string?, object?> _convert;

        private ValueConversion(IExpression<string?> reader, Func<string?, object?> convert)
        {
            _reader = reader;
            _convert = convert;
        }

        public static ValueConversion Explicit(IExpression<string?> reader, Func<string?, object?> convert)
        => new(reader, convert);

        public static ValueConversion Implicit(SchemaMember member, IExpression<string?> reader)
            => new(reader, member.Convert);

        public static ValueConversion None(IExpression<string?> reader)
            => new(reader, v => v);

        public object? Evaluate()
            => _convert(_reader.Evaluate());
    }
}
