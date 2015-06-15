using NUnit.Framework;
using System;

namespace GentleWare.Krypto.UnitTests
{
	[TestFixture]
	public class KryptoNodeTest
	{
		[Test, ExpectedException(typeof(NotSupportedException))]
		public void Create_InvalidOperator_ThrowsNotSupportedException()
		{
			var opr = (OperatorType)14;
			KryptoNode.Create(opr, new ValueNode(3), new ValueNode(2));
		}
	}
}
