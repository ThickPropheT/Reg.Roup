namespace TreeVal.Tests.Node;

using Condition;

[TestFixture]
public class UsingObjectEquals
{
    [TestFixture]
    public class HasEqualityWith
    {
        [Test]
        public void ObjectsEqualToItsValue()
        {
            var node0 = new Node(0);
            object value0 = 0;

            Assert.That(node0, Is.EqualTo(value0));
        }
    }

    [TestFixture]
    public class DoesNotHaveEqualityWith
    {
        [Test]
        public void ObjectsNotEqualToItsValue()
        {
            var node0 = new Node(0);
            object value1 = 1;

            Assert.That(node0, Is.Not.EqualTo(value1));
        }
        
        [Test]
        public void ObjectsWithDifferentTypeThanItsValue()
        {
            var node0 = new Node(0);
            object valueString = "string";

            Assert.That(node0, Is.Not.EqualTo(valueString));
        }
    }
}
