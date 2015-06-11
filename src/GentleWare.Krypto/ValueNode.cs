using System;
using System.Linq;
using System.Diagnostics;

namespace GentleWare.Krypto
{
	/// <summary>Represents a value node.</summary>
	[DebuggerDisplay("{m_Value}")]
	public struct ValueNode : IKryptoNode
	{
		private int m_Value;
		
		/// <summary>Creates a new instance of the node.</summary>
		public ValueNode(int val) { m_Value = val; }

		/// <summary>Gets the Int32 value of the node.</summary>
		public Int32 Value { get { return m_Value; } }

		/// <summary>Gets the (potentially) complexity of the node.</summary>
		public Double Complexity { get { return 250 - m_Value; } }

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate() { return new ValueNode(-m_Value); }

		/// <summary>Simplifies the node.</summary>
		public IKryptoNode Simplify() { return this; }

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString() { return m_Value.ToString(); }

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Gets a code for hashing purposes.</summary>
		public override int GetHashCode() { return m_Value; }

		public static IKryptoNode[] Create(params int[] values)
		{
			return values
				.Select(val => new ValueNode(val))
				.Cast<IKryptoNode>()
				.ToArray();
		}
	}
}
