namespace TreeVal.Tests.__Resources.Dummies;

public class DummyMethod
{
    public static DummyMethod Instance { get; } = new();
    
    public string GetString1() => string.Empty;
    public string GetString1(string arg) => arg;
    
    public string GetString2() => string.Empty;
    public string GetString2(string arg) => arg;

    public abstract class Static
    {
        public static string GetString1() => string.Empty;
        public static string GetString1(string arg) => arg;
        
        public static string GetString2() => string.Empty;
        public static string GetString2(string arg) => arg;
    }
}
