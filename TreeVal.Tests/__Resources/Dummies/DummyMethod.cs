namespace TreeVal.Tests.__Resources.Dummies;

public class DummyMethod
{
    public static DummyMethod Instance { get; } = new();
    
    public string GetString() => string.Empty;
    public string ReturnArg(string arg) => arg;

    public class Static
    {
        public static string GetString() => string.Empty;
        public static string ReturnArg(string arg) => arg;
    }
}
