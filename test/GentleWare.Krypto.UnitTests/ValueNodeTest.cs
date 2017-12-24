using NUnit.Framework;

namespace GentleWare.Krypto.UnitTests
{
    public class ValueNodeTest
    {
        public static readonly ValueNode TestNode = new ValueNode(17);

        [Test]
        public void Value_TestNode_17()
        {
            var act = TestNode.Value;
            var exp = 17;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Complexity_TestNode_267d()
        {
            var act = TestNode.Complexity;
            var exp = 267d;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Negate_TestNode_minus17()
        {
            var act = TestNode.Negate();
            var exp = new ValueNode(-17);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Simplify_TestNode_TestNode()
        {
            var act = TestNode.Simplify();
            var exp = TestNode;

            Assert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }

        [Test]
        public void ToString_TestNode_StringRepresentation()
        {
            var act = TestNode.ToString();
            var exp = "17";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetHashCode_TestNode_17()
        {
            var act = TestNode.GetHashCode();
            var exp = 17;

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
