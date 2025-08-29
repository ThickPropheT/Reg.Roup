namespace TreeVal.Tests.__Resources.Dummies;

public static class DummyExtensionMethod
{
    public static string GetString1(this object _, string arg, object? anything) => arg;
    public static string GetString3(this object _, string arg) => arg;
}
