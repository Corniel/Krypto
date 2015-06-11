using NUnit.Framework;
using System;

namespace GentleWare.Krypto.UnitTests
{
	[TestFixture]
	public class DivideNodeTest
	{
		public static readonly DivideNode TestNode = new DivideNode(16, 4);

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Ctor_DenominatorIsZero_ThrowsArgumentOutOfRangeException()
		{
			new DivideNode(new ValueNode(16), new ValueNode(0));
		}
		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Ctor_1DivideBy3_ThrowsArgumentOutOfRangeException()
		{
			new DivideNode(1, 3);
		}

		[Test]
		public void Value_TestNode_13()
		{
			var act = TestNode.Value;
			var exp = 4;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Complexity_TestNode_936d()
		{
			var act = TestNode.Complexity;
			var exp = 936.0;

			Assert.AreEqual(exp, act, 0.001);
		}

		[Test]
		public void Negate_4Divide2_AddNode2Minus4()
		{
			var node = new DivideNode(4, 2);
			var act = node.Negate();
			var exp = new NegationNode(new AddNode(4, -2));

			Assert.AreEqual(exp, act);
			Assert.AreNotSame(exp, act);
		}
		[Test]
		public void Negate_TestNode_Minus16Divid4()
		{
			var act = TestNode.Negate();
			var exp = new NegationNode(new DivideNode(16, 4));

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Negate_8DivdeMinus2_8Divide2()
		{
			var node = new DivideNode(8, -2);
			var act = node.Negate();
			var exp = new DivideNode(8, 2);

			Assert.AreEqual(exp, act);
			Assert.AreNotSame(exp, act);
		}
		[Test]
		public void Negate_0DivideMinus4_4Multiply0()
		{
			var node = new DivideNode(0, -4);
			var act = node.Negate();
			var exp = new MultiplyNode(0, 4);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Negate_0Divide4_4Multiply0()
		{
			var node = new DivideNode(0, 4);
			var act = node.Negate();
			var exp = new MultiplyNode(0, 4);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Negate_17Divide1_Minus17Multiply1()
		{
			var node = new DivideNode(17, 1);
			var act = node.Negate();
			var exp = new NegationNode(new MultiplyNode(17, 1));

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Negate_17DivideMinus1_17Multiply1()
		{
			var node = new DivideNode(17, -1);
			var act = node.Negate();
			var exp = new MultiplyNode(17, 1);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Simplify_4Divide2_AddNode4Minus2()
		{
			var node = new DivideNode(4, 2);
			var act = node.Simplify();
			var exp = new AddNode(4, -2);

			Assert.AreEqual(exp, act);
			Assert.AreNotSame(exp, act);
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
		public void Simplify_8DivdeMinus2_Minus8Divide2()
		{
			var node = new DivideNode(8, -2);
			var act = node.Simplify();
			var exp = new NegationNode(new DivideNode(8, 2));

			Assert.AreEqual(exp, act);
			Assert.AreNotSame(exp, act);
		}
		[Test]
		public void Simplify_0DivideMinus4_4Multiply0()
		{
			var node = new DivideNode(0, 4);
			var act = node.Simplify();
			var exp = new MultiplyNode(0, 4);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Simplify_0Divide4_4Multiply0()
		{
			var node = new DivideNode(0, 4);
			var act = node.Simplify();
			var exp = new MultiplyNode(0, 4);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Simplify_17Divide1_17Multiply1()
		{
			var node = new DivideNode(17, 1);
			var act = node.Simplify();
			var exp = new MultiplyNode(17, 1);

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Simplify_17DivideMinus1_Minus17Multiply1()
		{
			var node = new DivideNode(17, -1);
			var act = node.Simplify();
			var exp = new NegationNode(new MultiplyNode(17, 1));

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void ToString_TestNode_StringRepresentation()
		{
			var act = TestNode.ToString();
			var exp = "(16 / 4)";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetHashCode_TestNode_67108880()
		{
			var act = TestNode.GetHashCode();
			var exp = 67108880;

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
