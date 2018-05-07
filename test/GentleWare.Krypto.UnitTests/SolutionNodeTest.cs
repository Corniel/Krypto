using NUnit.Framework;
using System;

namespace GentleWare.Krypto.UnitTests
{
    public class SolutionNodeTest
    {
        [Test]
        public void Negate_SolutionNode_ThrowsNotSupportedException()
        {
            var node = new SolutionNode(new AddNode(2, 3));
            Assert.Throws<NotSupportedException>(() =>
            {
                node.Negate();
            });
        }

        [Test]
        public void ToString_None_NoSolutions()
        {
            var act = SolutionNode.None.ToString();
            var exp = "No solutions";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Simplify_5x1p4x3m2()
        {
            var node = new SolutionNode
            (
                new MultiplyNode
                (
                    new ValueNode(5),
                    new AddNode(new MultiplyNode(1), new ValueNode(4)),
                    new AddNode(3, -2)
                )
            );
            var act = node.Simplify().Simplify().Simplify();
            Assert.AreEqual("25 = 5 * (1 + 4) * (3 - 2)", act.ToString());
        }
    }
}
