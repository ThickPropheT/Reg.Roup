namespace TreeVal.Tests.__Resources.Dummies;

public static class DummyStaticMethod
{
    public static string GetString1() => string.Empty;
    public static string GetString1(string arg) => arg;
        
    public static string GetString2() => string.Empty;
    public static string GetString2(string arg) => arg;

    public static string GetString3(this object _, string arg) => arg;
}
