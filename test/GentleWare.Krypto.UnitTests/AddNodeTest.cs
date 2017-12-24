using NUnit.Framework;

namespace GentleWare.Krypto.UnitTests
{
    public class AddNodeNodeTest
    {
        public static readonly AddNode TestNode = new AddNode(
            new ValueNode(17),
            new ValueNode(-3),
            new SubtractNode(2, 3));

        [Test]
        public void Value_TestNode_13()
        {
            var act = TestNode.Value;
            var exp = 13;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Complexity_TestNode_1284d215()
        {
            var act = TestNode.Complexity;
            var exp = 1284.215;

            Assert.AreEqual(exp, act, 0.001);
        }

        [Test]
        public void Negate_TestNode_2p17m3m3()
        {
            var act = TestNode.Negate();
            var exp = new AddNode(3, 3, -17, -2);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Simplify_Plus17_17()
        {
            var node = new AddNode(17);
            var act = node.Simplify();
            var exp = new ValueNode(17);

            Assert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }

        [Test]
        public void Simplify_TestNode_2p17m3m3()
        {
            var act = TestNode.Simplify();
            var exp = new AddNode(2, 17, -3, -3);

            Assert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }

        [Test]
        public void ToString_TestNode_StringRepresentationPlus3Plus2Minus3()
        {
            var act = TestNode.ToString();
            var exp = "(17 - 3 + (2 - 3))";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetHashCode_TestNode_Minus3145787()
        {
            var act = TestNode.GetHashCode();
            var exp = -3145787;

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
