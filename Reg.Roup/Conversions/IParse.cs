using System;

namespace Reg.Roup.Conversions
{
    public interface IParse
    {
        public T With<T>(Func<string, T> parse) => default!;
    }
}
