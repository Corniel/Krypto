using NUnit.Framework;
using System;

namespace GentleWare.Krypto.UnitTests
{
    public class KryptoNodeTest
    {
        [Test]
        public void Create_InvalidOperator_ThrowsNotSupportedException()
        {
            var opr = (OperatorType)14;
            Assert.Throws<NotSupportedException>(() =>
            {
                KryptoNode.Create(opr, new ValueNode(3), new ValueNode(2));
            });
        }
    }
}
