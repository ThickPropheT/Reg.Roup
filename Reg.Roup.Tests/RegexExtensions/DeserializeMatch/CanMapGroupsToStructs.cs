using Reg.Roup.Tests.Resources.TestScenarios;
using Reg.Roup.Tests.Resources.TestScenarios.Default;
using static Reg.Roup.Tests.Resources.TestScenarios.Default.DefaultSchema;

namespace Reg.Roup.Tests.RegexExtensions.DeserializeMatch
{
    [TestFixture]
    public class CanMapGroupsToStructs
    {
        private GivenTextMatchedByRegex<Expected> _givenTextMatchedByRegex;

        [OneTimeSetUp]
        public void SetUp()
        {
            _givenTextMatchedByRegex = new DefaultScenario();
        }

        [Test]
        public void UsingConstructorParameters()
        {
            _givenTextMatchedByRegex
                .WhenMatchDeserializedVia(parse => new SchemaStruct("", 0, false, parse.With(Version.Parse)))
                .AssertMultiple((expected, actual) =>
                {
                    Assert.That(actual.name, Is.EqualTo(expected.Name));
                    Assert.That(actual.index, Is.EqualTo(expected.Index));
                    Assert.That(actual.isEnabled, Is.EqualTo(expected.IsEnabled));
                    Assert.That(actual.version, Is.EqualTo(expected.Version));
                });
        }

        [Test]
        public void UsingObjectInitializers()
        {
            _givenTextMatchedByRegex
                .WhenMatchDeserializedVia(parse => new SchemaStruct { name = "", index = 0, isEnabled = false, version = parse.With(Version.Parse) })
                .AssertMultiple((expected, actual) =>
            {
                Assert.That(actual.name, Is.EqualTo(expected.Name));
                Assert.That(actual.index, Is.EqualTo(expected.Index));
                Assert.That(actual.isEnabled, Is.EqualTo(expected.IsEnabled));
                Assert.That(actual.version, Is.EqualTo(expected.Version));
            });
        }

        [Test]
        public void UsingMixedBindings()
        {
            _givenTextMatchedByRegex
                .WhenMatchDeserializedVia(parse => new SchemaStruct(parse.With(Version.Parse)) { name = "", index = 0, isEnabled = false })
                .AssertMultiple((expected, actual) =>
            {
                Assert.That(actual.name, Is.EqualTo(expected.Name));
                Assert.That(actual.index, Is.EqualTo(expected.Index));
                Assert.That(actual.isEnabled, Is.EqualTo(expected.IsEnabled));
                Assert.That(actual.version, Is.EqualTo(expected.Version));
            });
        }
    }
}
