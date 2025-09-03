namespace TreeVal.Primitives;

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

    public static bool AreEqual(EType type, object? other)
        => type.Equals(other);

    private static bool StrictEquality(Type t1, Type t2) => t1 == t2;
    private static bool LooseEquality(Type t1, Type t2) => t1.IsAssignableTo(t2);

    public override bool Equals(object? other)
    {
        if (other is null) return false;
        // if other is this value wrapper
        if (ReferenceEquals(other, this)) return true;
        // if other is the value wrapped by this wrapper
        if (ReferenceEquals(other, _inner)) return true;
        // if other is a different value wrapper
        if (other is EType otherWrapper) return _isEqualTo(otherWrapper._inner, _inner);
        // if other is a value wrapped by a different wrapper
        if (other is Type otherType) return _isEqualTo(otherType, _inner);

        return false;
    }

    public override int GetHashCode()
        => _inner.GetHashCode();

    public override string ToString()
        => _inner.ToString();
}
