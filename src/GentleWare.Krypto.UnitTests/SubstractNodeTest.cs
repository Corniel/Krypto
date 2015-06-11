using NUnit.Framework;

namespace GentleWare.Krypto.UnitTests
{
	[TestFixture]
	public class SubtractNodeTest
	{
		public static readonly SubtractNode TestNode = new SubtractNode(17, 4);

		[Test]
		public void Value_TestNode_13()
		{
			var act = TestNode.Value;
			var exp = 13;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Complexity_TestNode_718d5()
		{
			var act = TestNode.Complexity;
			var exp = 718.5;

			Assert.AreEqual(exp, act, 0.001);
		}

		[Test]
		public void Negate_TestNode_Minus4Add17()
		{
			var act = TestNode.Negate();
			var exp = new AddNode(4, -17);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Simplify_TestNode_AddNode()
		{
			var act = TestNode.Simplify();
			var exp = new AddNode(new ValueNode(17), new ValueNode(-4));

			Assert.AreEqual(exp, act);
			Assert.AreNotSame(exp, act);
		}

		[Test]
		public void Simplify_NodeSmallerThenZero_NegatedAddNode()
		{
			var node = new SubtractNode(4, 17);
			var act = node.Simplify();
			var exp = new AddNode(4, -17);

			Assert.AreEqual(exp, act);
			Assert.AreNotSame(exp, act);
		}

		[Test]
		public void ToString_TestNode_StringRepresentation()
		{
			var act = TestNode.ToString();
			var exp = "(17 - 4)";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetHashCode_TestNode_262161()
		{
			var act = TestNode.GetHashCode();
			var exp = 262161;

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
