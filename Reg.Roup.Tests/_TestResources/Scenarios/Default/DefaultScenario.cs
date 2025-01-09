using System.Text.RegularExpressions;

namespace Reg.Roup.Tests._TestResources.Scenarios.Default
{
    public partial class DefaultScenario(DefaultSchema.Expected? expected = null)
        : GivenTextMatchedByRegex<DefaultSchema.Expected>(expected ?? DefaultInput, () => ToText(expected ?? DefaultInput), GenerateRegex())
    {
        [GeneratedRegex("{ \"name\": \"(?<name>(\\w|\\d|\\.|-|_)+)\", \"version\": \"(?<version>\\d+\\.\\d+\\.\\d+)\", \"index\": (?<index>\\d+), \"isEnabled\": (?<isEnabled>true|false) }", RegexOptions.Compiled)]
        public static partial Regex GenerateRegex();

        public static string ToText(DefaultSchema.Expected i)
            => @$"{{ ""name"": ""{i.Name}"", ""version"": ""{i.Version}"", ""index"": {i.Index}, ""isEnabled"": {i.IsEnabled.ToString().ToLower()} }}";

        public static DefaultSchema.Expected DefaultInput { get; }
            = new()
            {
                Name = "Reg.Roup",
                Index = 1,
                IsEnabled = true,
                Version = new Version("1.0.0")
            };
    }
}

