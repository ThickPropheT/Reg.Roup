namespace TreeVal;

public class EValue
{
    private readonly object? _inner;
    readonly Func<object?, object?, bool> _isEqualTo;

    protected EValue(object? inner, Func<object?, object?, bool>? isEqualTo = null)
    {
        if (inner is EValue e)
        {
            inner = e._inner;
            isEqualTo = e._isEqualTo;
        }

        if (inner is not null && isEqualTo is null)
        {
            isEqualTo = inner.GetType().IsPrimitive
                ? Equals
                : ReferenceEquals;
        }
        
        _inner = inner;
        _isEqualTo = isEqualTo ?? Equals;
    }

    public static EValue Null() => new(null, Equals);
    public static EValue EqualTo<T>(T? value) => new(value, Equals);
    public static EValue ReferenceEqualTo<T>(T? value) => new(value, ReferenceEquals);
    
    public bool IsEqualTo(object? other)
    {
        if (other is null) return _inner is null;
        return _isEqualTo(other, _inner);
    }
}

public class EValue<T> : EValue
{
    private EValue(object? inner, Func<object?, object?, bool>? isEqualTo = null)
        : base(inner, isEqualTo)
    {
    }

    public static implicit operator EValue<T>(T? value) => new(value);
}
