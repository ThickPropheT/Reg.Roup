using System.Diagnostics;

namespace TreeVal.Media;

[DebuggerDisplay("{Value}")]
public class Node : IEquatable<Node>
{
    public object Value { get; }

    public Node(object value)
    {
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is Node n) return Equals(n);
        return Value.Equals(obj);
    }

    public bool Equals(Node? other)
    {
        return !Equals(other, null)
               && Value.Equals(other.Value);
    }

    public static bool operator ==(Node? left, Node? right)
        => EqualsImpl(left, right);

    public static bool operator ==(Node? left, object? right)
        => EqualsImpl(left, right);

    public static bool operator ==(object? left, Node? right)
        => EqualsImpl(right, left);

    public static bool operator !=(Node? left, Node? right)
        => !EqualsImpl(left, right);

    public static bool operator !=(Node? left, object? right)
        => !EqualsImpl(left, right);

    public static bool operator !=(object? left, Node? right)
        => !EqualsImpl(right, left);

    private static bool EqualsImpl(Node? left, object? right)
    {
        return left?.Equals(right)
               ?? Equals(right, null);
    }

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value.ToString()!;
}

[DebuggerDisplay("{Value}")]
public class Node<T> : Node
{
    public new T Value => (T) base.Value;

    public Node(T value)
        : base(value!)
    {
    }
}
