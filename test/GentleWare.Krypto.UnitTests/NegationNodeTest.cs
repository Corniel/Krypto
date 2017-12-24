using NUnit.Framework;

namespace GentleWare.Krypto.UnitTests
{
    [TestFixture]
    public class NegationNodeTest
    {
        public static readonly NegationNode TestNode = new NegationNode(new ValueNode(17));

        [Test]
        public void Value_TestNode_17()
        {
            var act = TestNode.Value;
            var exp = -17;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Complexity_TestNode_317d73()
        {
            var act = TestNode.Complexity;
            var exp = 317.73d;

            Assert.AreEqual(exp, act, 0.001);
        }

        [Test]
        public void Negate_TestNode_17()
        {
            var act = TestNode.Negate();
            var exp = new ValueNode(17);

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Negate_DoubleNegate17_minus17()
        {
            var node = new NegationNode(TestNode);

            var act = node.Negate();
            var exp = new ValueNode(-17);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Simplify_TestNode_TestNode()
        {
            var act = TestNode.Simplify();
            var exp = new ValueNode(-17);

            Assert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }
        [Test]
        public void Simplify_DoubleNegate17_17()
        {
            var node = new NegationNode(TestNode);

            var act = node.Simplify();
            var exp = new ValueNode(17);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestNode_StringRepresentation()
        {
            var act = TestNode.ToString();
            var exp = "-17";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetHashCode_TestNode_54882080()
        {
            var act = TestNode.GetHashCode();
            var exp = 54882080;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Equals_TestNodeEqualsTestNode_IsTrue()
        {
            var l = TestNode;
            var r = TestNode;

            Assert.IsTrue(l.Equals(r));
            Assert.AreNotSame(l, r);
        }
    }
}
