using NUnit.Framework;

namespace GentleWare.Krypto.UnitTests
{
    [TestFixture]
    public class MultiplyNodeTest
    {
        public static readonly MultiplyNode TestNode = new MultiplyNode(
            new ValueNode(17),
            new ValueNode(-3),
            new SubtractNode(2, 3));

        [Test]
        public void Value_TestNode_51()
        {
            var act = TestNode.Value;
            var exp = 51;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Complexity_TestNode_1589d375()
        {
            var act = TestNode.Complexity;
            var exp = 1589.375;

            Assert.AreEqual(exp, act, 0.001);
        }

        [Test]
        public void Negate_4x2xMin3_2x3x4()
        {
            var node = new MultiplyNode(4, 2, -3);
            var act = node.Negate();
            var exp = new MultiplyNode(2, 3, 4);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Negate_4x3_min3x4()
        {
            var node = new MultiplyNode(4, 3);
            var act = node.Negate();
            var exp = new NegationNode(new MultiplyNode(3, 4));

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Negate_4xmin3_3x4()
        {
            var node = new MultiplyNode(4, -3);
            var act = node.Negate();
            var exp = new MultiplyNode(3, 4);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Simplify_NodeWithOneChild_SingleNode()
        {
            var node = new MultiplyNode(new ValueNode(17));
            var act = node.Simplify();
            var exp = new ValueNode(17);

            Assert.AreEqual(exp, act);
            Assert.AreNotSame(exp, act);
        }

        [Test]
        public void Simplify_ComplexExpression()
        {
            // 24 = (((5 * 3) - (17 - 4)) * 12)
            var node = new MultiplyNode(new SubtractNode(new MultiplyNode(5, 3), new SubtractNode(17, 4)), new ValueNode(12));
            IKryptoNode act = new SolutionNode(node);

            for (var i = 0; i < 4; i++)
            {
                act = act.Simplify();
                Assert.AreEqual(24, act.Value, i.ToString());
            }
        }

        [Test]
        public void ToString_TestNode_m3x2m3x17()
        {
            var act = TestNode.ToString();
            var exp = "(-3 * (2 - 3) * 17)";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetHashCode_TestNode_m393283()
        {
            var act = TestNode.GetHashCode();
            var exp = -393283;

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
