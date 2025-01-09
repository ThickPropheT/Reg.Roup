using Reg.Roup.Tests._TestResources.Scenarios;
using Reg.Roup.Tests._TestResources.Scenarios.Default;
using DefaultScenario = Reg.Roup.Tests._TestResources.Scenarios.Default.DefaultScenario;

namespace Reg.Roup.Tests.RegexExtensions.DeserializeMatch
{
    [TestFixture]
    public class CanMapGroupsToAnonymousTypes
    {
        private GivenTextMatchedByRegex<DefaultSchema.Expected> _givenTextMatchedByRegex;

        [OneTimeSetUp]
        public void SetUp()
        {
            _givenTextMatchedByRegex = new DefaultScenario();
        }

        [Test]
        public void UsingObjectInitializers()
        {
            _givenTextMatchedByRegex
                .WhenMatchDeserializedVia(parse => new { name = "", index = 0, isEnabled = false, version = parse.With(Version.Parse) })
                .Assert((expected, actual) =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(actual.name, Is.EqualTo(expected.Name));
                        Assert.That(actual.index, Is.EqualTo(expected.Index));
                        Assert.That(actual.isEnabled, Is.EqualTo(expected.IsEnabled));
                        Assert.That(actual.version, Is.EqualTo(expected.Version));
                    });
                });
        }
    }
}
