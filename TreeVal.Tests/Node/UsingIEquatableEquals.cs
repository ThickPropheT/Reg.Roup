namespace TreeVal.Tests.Node;

[TestFixture]
public class UsingIEquatableEquals
{
    [TestFixture]
    public class HasEqualityWith
    {
        [Test]
        public void NodesWhereValueIsEqual()
        {
            var node0 = new Media.Node(0);
            var otherNode0 = new Media.Node(0);

            Assert.That(node0, Is.EqualTo(otherNode0));
        }
    }

    [TestFixture]
    public class DoesNotHaveEqualityWith
    {
        [Test]
        public void NodesWhereValueIsDifferent()
        {
            var node0 = new Media.Node(0);
            var otherNode1 = new Media.Node(1);

            Assert.That(node0, Is.Not.EqualTo(otherNode1));
        }

        [Test]
        public void NodesWhereValueIsDifferentType()
        {
            var node0 = new Media.Node(0);
            var otherNodeString = new Media.Node("string");

            Assert.That(node0, Is.Not.EqualTo(otherNodeString));
        }
    }
}
