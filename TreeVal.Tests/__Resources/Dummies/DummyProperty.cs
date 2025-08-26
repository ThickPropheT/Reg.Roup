namespace TreeVal.Tests.__Resources.Dummies;

public class DummyProperty
{
    public static DummyProperty Instance { get; } = new();

    public string String { get; } = string.Empty;

    public class Static
    {
        public string String { get; } = string.Empty;
    }
}
