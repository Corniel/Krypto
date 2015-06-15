using System;
using System.Collections.Generic;

namespace GentleWare.Krypto
{
	public interface IKryptoNode
	{
		/// <summary>Gets the Int32 value of the node.</summary>
		Int32 Value { get; }

		/// <summary>Gets the (potentially) complexity of the node.</summary>
		Double Complexity { get; }

		/// <summary>Returns true the node more then one value node.</summary>
		bool IsComplex { get; }

		/// <summary>Negates the node.</summary>
		IKryptoNode Negate();

		/// <summary>Simplifies the node.</summary>
		IKryptoNode Simplify();

		/// <summary>Gets the underlying value nodes.</summary>
		IEnumerable<ValueNode> GetValueNodes();
	}
}
