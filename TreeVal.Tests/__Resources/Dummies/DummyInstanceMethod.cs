namespace TreeVal.Tests.__Resources.Dummies;

public class DummyInstanceMethod
{
    public static DummyInstanceMethod Instance { get; } = new();
    
    public string GetString1() => string.Empty;
    public string GetString1(string arg) => arg;
    
    public string GetString2() => string.Empty;
    public string GetString2(string arg) => arg;
}
