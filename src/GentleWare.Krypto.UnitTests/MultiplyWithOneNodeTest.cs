using NUnit.Framework;

namespace GentleWare.Krypto.UnitTests
{
	[TestFixture]
	public class MultiplyWithOneNodeTest
	{
		[Test]
		public void Simplify_4m2x1_4m2x1()
		{
			var node = new SubtractNode(new ValueNode(4), new MultiplyNode(2, 1));

			var act = node.Simplify().Simplify();
			var exp = new MultiplyWithOneNode(new AddNode(4, -2), new ValueNode(1));

			Assert.AreEqual(exp, act);

		}
	}
}
