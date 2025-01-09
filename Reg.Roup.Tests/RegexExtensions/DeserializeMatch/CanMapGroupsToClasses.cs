using Reg.Roup.Tests._TestResources.Scenarios;
using Reg.Roup.Tests._TestResources.Scenarios.Default;
using static Reg.Roup.Tests._TestResources.Scenarios.Default.DefaultSchema;
using DefaultScenario = Reg.Roup.Tests._TestResources.Scenarios.Default.DefaultScenario;

namespace Reg.Roup.Tests.RegexExtensions.DeserializeMatch
{
    [TestFixture]
    public class CanMapGroupsToClasses
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
                .WhenMatchDeserializedVia(parse => new SchemaClass("", 0, false, parse.With(s => Version.Parse(s))))
                .AssertActualEqualsExpected();
        }

        [Test]
        public void UsingObjectInitializers()
        {
            _givenTextMatchedByRegex
                .WhenMatchDeserializedVia(parse => new SchemaClass { name = "", index = 0, isEnabled = false, version = parse.With(Version.Parse) })
                .AssertActualEqualsExpected();
        }

        [Test]
        public void UsingMixedBindings()
        {
            _givenTextMatchedByRegex
                .WhenMatchDeserializedVia(parse => new SchemaClass("") { index = 0, isEnabled = false, version = parse.With(Version.Parse) })
                .AssertActualEqualsExpected();
        }
    }
}
