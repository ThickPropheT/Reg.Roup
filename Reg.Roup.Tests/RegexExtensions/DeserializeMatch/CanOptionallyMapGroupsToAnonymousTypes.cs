using Reg.Roup.Tests.Resources.TestScenarios.OptionalMapping;

namespace Reg.Roup.Tests.RegexExtensions.DeserializeMatch
{
    [TestFixture]
    public class CanOptionallyMapGroupsToAnonymousTypes
    {
        [Test]
        public void ByDeserializingUndefinedMembersToNull()
        {
            OptionalMappingScenario
                .WithTextFrom(new() { Number = 0, Text = "" })
                .WhenMatchDeserializedVia(parse => new { undefined = (string?)null })
                .Assert((_, actual) =>
                {
                    Assert.That(actual.undefined, Is.Null);
                });
        }

        [Test]
        public void ByDeserializingMatchesToMemberTypes()
        {
            OptionalMappingScenario
                .WithTextFrom(new() { Text = "NotNull", Number = 42 })
                .WhenMatchDeserializedVia(parse => new { text = (string?)null, number = (int?)null })
                .Assert((expected, actual) =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(actual.text, Is.EqualTo(expected.Text));
                        Assert.That(actual.number, Is.EqualTo(expected.Number));
                    });
                });
        }

        [Test]
        public void ByDeserializingUnmatchedMembersToNull()
        {
            OptionalMappingScenario
                .WithTextFrom(new() { Text = null, Number = null })
                .WhenMatchDeserializedVia(parse => new { text = (string?)null, number = (int?)null })
                .Assert((expected, actual) =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(actual.text, Is.Null);
                        Assert.That(actual.number, Is.Null);
                    });
                });
        }

        [Ignore("b/c")]
        [Test]
        public void Doh1()
        {
            // TODO
            //  !!! type.CanBeAssignedNull() erroneously causes strings to be marked optional.
            //  this test fails b/c GroupValueConversion sees the "optional" string and converts "" -> null

            OptionalMappingScenario
                .WithTextFrom(new() { Text = null, Number = null })
                .WhenMatchDeserializedVia(parse => new { text = "", number = (int?)null })
                .Assert((expected, actual) =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(actual.text, Is.Empty);
                    });
                });
        }

        [Ignore("b/c")]
        [Test]
        public void Doh2()
        {
            // TODO
            //  this throws incorrect exception w/ \d*
            //  \d* counts as matching the group even when there is no number,
            //  and therefore doesn't trip the normal "required" checks

            // TODO using IndexOutOfRangeException until a satistfactory exception is thrown by the implementation
            Assert.Throws<IndexOutOfRangeException>(() =>
                OptionalMappingScenario
                    .WithTextFrom(new() { Text = "NotNull", Number = null })
                    .WhenMatchDeserializedVia(parse => new { text = "", number = 0 })
                );
        }
    }
}
