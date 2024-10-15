using System.Text.RegularExpressions;
using static Reg.Roup.Tests.Resources.TestScenarios.OptionalMapping.OptionalMappingSchema;

namespace Reg.Roup.Tests.Resources.TestScenarios.OptionalMapping
{
    public partial class OptionalMappingScenario(Expected expected)
        : GivenTextMatchedByRegex<Expected>(expected, () => ToText(expected), GenerateRegex())
    {
        [GeneratedRegex("text: (?<text>\\w*), number: (?<number>\\d+)", RegexOptions.Compiled)]
        public static partial Regex GenerateRegex();

        public static string ToText(Expected e)
            => @$"text: {e.Text}, number: {e.Number}";

        public static GivenTextMatchedByRegex<Expected> WithTextFrom(Expected expected)
            => new OptionalMappingScenario(expected);
    }
}
