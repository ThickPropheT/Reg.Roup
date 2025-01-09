using static Reg.Roup.Tests._TestResources.Scenarios.Default.DefaultSchema;

namespace Reg.Roup.Tests._TestResources.Scenarios.Default
{
    public static class DefaultSchema
    {
        public readonly struct Expected
        {
            public string Name { get; init; }
            public int Index { get; init; }
            public bool IsEnabled { get; init; }
            public Version Version { get; init; }
        }

        public class SchemaClass
        {
            public string? name { get; init; }
            public int? index { get; init; }
            public bool? isEnabled { get; init; }
            public Version? version { get; init; }

            public SchemaClass() { }

            public SchemaClass(string name)
            {
                this.name = name;
            }

            public SchemaClass(Version version)
            {
                this.version = version;
            }

            public SchemaClass(string name, int index, bool isEnabled, Version version)
            {
                this.name = name;
                this.index = index;
                this.isEnabled = isEnabled;
                this.version = version;
            }
        }

        public readonly struct SchemaStruct
        {
            public string? name { get; init; }
            public int? index { get; init; }
            public bool? isEnabled { get; init; }
            public Version? version { get; init; }

            public SchemaStruct(string name)
            {
                this.name = name;
            }

            public SchemaStruct(Version version)
            {
                this.version = version;
            }

            public SchemaStruct(string name, int index, bool isEnabled, Version version)
            {
                this.name = name;
                this.index = index;
                this.isEnabled = isEnabled;
                this.version = version;
            }
        }
    }

    public static class RegexDeserializationResultExtensions
    {
        public static void AssertActualEqualsExpected(this RegexDeserializationResult<Expected, SchemaClass> result)
            => result.Assert((expected, actual) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.name, Is.EqualTo(expected.Name));
                    Assert.That(actual.index, Is.EqualTo(expected.Index));
                    Assert.That(actual.isEnabled, Is.EqualTo(expected.IsEnabled));
                    Assert.That(actual.version, Is.EqualTo(expected.Version));
                });
            });

        public static void AssertActualEqualsExpected(this RegexDeserializationResult<Expected, SchemaStruct> result)
            => result.Assert((expected, actual) =>
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

