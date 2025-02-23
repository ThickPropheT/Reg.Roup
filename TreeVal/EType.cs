namespace TreeVal;

public class EType
{
    private readonly Type _inner;
    private readonly Func<Type, Type, bool> _isEqualTo;

    private EType(Type inner, Func<Type, Type, bool>? isEqualTo = null)
    {
        _inner = inner;

        if (isEqualTo != null)
        {
            _isEqualTo = isEqualTo;
            return;
        }

        _isEqualTo = inner.IsPrimitive
            ? StrictEquality
            : LooseEquality;
    }

    public static implicit operator EType(Type type) => new(type);

    public static EType EqualTo(Type type) => new(type, StrictEquality);
    public static EType EqualTo<T>() => new(typeof(T), StrictEquality);
    public static EType Is(Type type) => new(type, LooseEquality);
    public static EType Is<T>() => new(typeof(T), LooseEquality);

    public bool IsEqualTo(object other)
        => other is Type otherType
           && _isEqualTo(otherType, _inner);

    private static bool StrictEquality(Type t1, Type t2) => t1 == t2;
    private static bool LooseEquality(Type t1, Type t2) => t1.IsAssignableTo(t2);
}
