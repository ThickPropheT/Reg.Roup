namespace TreeVal;

public class EValue
{
    protected readonly object? Inner;
    protected readonly Func<object?, object?, bool> IsEqualTo;

    protected EValue(object? inner, Func<object?, object?, bool>? isEqualTo = null)
    {
        if (inner is EValue e)
        {
            inner = e.Inner;
            isEqualTo = e.IsEqualTo;
        }

        if (inner is not null && isEqualTo is null)
        {
            isEqualTo = inner.GetType().IsPrimitive
                ? Equals
                : ReferenceEquals;
        }

        Inner = inner;
        IsEqualTo = isEqualTo ?? Equals;
    }

    public static EValue Null() => new(null, Equals);
    public static EValue<T?> Null<T>() => EValue<T>.Null();
    public static EValue<T?> EqualTo<T>(T? value) => EValue<T?>.EqualTo(value);
    public static EValue<T?> ReferenceEqualTo<T>(T? value) => EValue<T?>.ReferenceEqualTo(value);

    public static bool AreEqual(object? other, EValue value)
        => value.Equals(other);

    public static bool AreEqual(EValue value, object? other)
        => value.Equals(other);

    public override bool Equals(object? other)
    {
        if (other is null) return Inner is null;
        // if other is this value wrapper
        if (ReferenceEquals(other, this)) return true;
        // if other is the value this wraps
        if (ReferenceEquals(other, Inner)) return true;
        // if other is a different value wrapper
        if (other is EValue otherWrapper) return Equals(otherWrapper);
        // if other is a value this doesn't wrap
        return IsEqualTo(other, Inner);
    }

    protected bool Equals(EValue other)
        => IsEqualTo(other.Inner, Inner);

    public override int GetHashCode()
        => Inner != null ? Inner.GetHashCode() : 0;

    public override string? ToString()
        => Inner?.ToString();
}

public class EValue<T> : EValue
{
    private EValue(object? inner, Func<object?, object?, bool>? isEqualTo = null)
        : base(inner, isEqualTo)
    {
    }

    public new static EValue<T?> Null() => new(null, Equals);
    public static EValue<T?> EqualTo(T? value) => new(value, Equals);
    public static EValue<T?> ReferenceEqualTo(T? value) => new(value, ReferenceEquals);

    public static implicit operator EValue<T>(T? value) => new(value);

    public override bool Equals(object? other)
    {
        if (other is null) return Inner is null;
        // if other is this value wrapper
        if (ReferenceEquals(other, this)) return true;
        // if other is the value wrapped by this wrapper
        if (ReferenceEquals(other, Inner)) return true;
        // if other is a different value wrapper
        if (other is EValue<T> otherWrapper) return Equals(otherWrapper);
        // if other is a value wrapped by a different wrapper
        if (other is T otherValue) return IsEqualTo(otherValue, Inner);

        return false;
    }

    public override int GetHashCode()
        => Inner != null ? Inner.GetHashCode() : 0;
}
