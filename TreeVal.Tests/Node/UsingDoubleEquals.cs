namespace TreeVal.Tests.Node;

using Condition;

[TestFixture]
public class UsingDoubleEquals
{
    [TestFixture]
    public class HasEquality
    {
        [Test]
        public void WithNodesWhereValueIsEqual()
        {
            var node0 = new Media.Node(0);
            var otherNode0 = new Media.Node(0);

            Assert.That(node0, Is.EqualTo(otherNode0).Using<Media.Node>((t1, t2) => t1 == t2));
        }

        [Test]
        public void BetweenNodeAndObjectsEqualToItsValue()
        {
            var node0 = new Media.Node(0);
            object value0 = 0;

            Assert.That(node0, Is.EqualTo(value0).Using<Media.Node, object>((t1, t2) => t1 == t2));
        }

        [Test]
        public void BetweenValueAndNodeWithEqualValue()
        {
            var value0 = 0;
            var node0 = new Media.Node(0);

            Assert.That(value0, Is.EqualTo(node0).Using<object, Media.Node>((t1, t2) => t1 == t2));
        }
    }

    [TestFixture]
    public class DoesNotHaveEquality
    {
        [Test]
        public void WithNodesWhereValueIsNotEqual()
        {
            var node0 = new Media.Node(0);
            var node1 = new Media.Node(1);

            Assert.That(node0, Is.Not.EqualTo(node1).Using<Media.Node>((t1, t2) => t1 == t2));
        }

        [Test]
        public void WithNodesWhereValueIsDifferentType()
        {
            var node0 = new Media.Node(0);
            var nodeString = new Media.Node("string");

            Assert.That(node0, Is.Not.EqualTo(nodeString).Using<Media.Node>((t1, t2) => t1 == t2));
        }

        [Test]
        public void BetweenNodeAndObjectsDifferentThanItsValue()
        {
            var node0 = new Media.Node(0);
            object value1 = 1;

            Assert.That(node0, Is.Not.EqualTo(value1).Using<Media.Node, object>((t1, t2) => t1 == t2));
        }

        [Test]
        public void BetweenNodeAndObjectsWithDifferentTypeThanItsValue()
        {
            var node0 = new Media.Node(0);
            object valueString = "string";

            Assert.That(node0, Is.Not.EqualTo(valueString).Using<Media.Node, object>((t1, t2) => t1 == t2));
        }

        [Test]
        public void BetweenObjectsAndNodeWithDifferentValue()
        {
            object value0 = 0;
            var node1 = new Media.Node(1);

            Assert.That(value0, Is.Not.EqualTo(node1).Using<object, Media.Node>((t1, t2) => t1 == t2));
        }

        [Test]
        public void BetweenObjectsAndNodeWithValueOfDifferentType()
        {
            object valueString = "string";
            var node0 = new Media.Node(0);

            Assert.That(valueString, Is.Not.EqualTo(node0).Using<object, Media.Node>((t1, t2) => t1 == t2));
        }
    }
}
