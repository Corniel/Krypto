using Qowaiv;
using System;

namespace GentleWare.Krypto
{
	/// <summary>Represents a subtract node of the form (a - b).</summary>
	/// <remarks>
	/// Both <see cref="Simplify()"/> and <see cref="Negate()"/> convert the
	/// subtract node to an add node.
	/// </remarks>
	public struct SubtractNode : IKryptoNode
	{
		/// <summary>Underlying node.</summary>
		private IKryptoNode Left;
		private IKryptoNode Right;

		/// <summary>Creates a new instance of the node.</summary>
		public SubtractNode(int nominator, int denominator) 
			: this(new ValueNode(nominator), new ValueNode(denominator)) { }

		/// <summary>Creates a new instance of the node.</summary>
		public SubtractNode(IKryptoNode left, IKryptoNode right)
		{
			Left = Guard.NotNull(left, "left");
			Right = Guard.NotNull(right, "right"); 
		}

		/// <summary>Gets the Int32 value of the node.</summary>
		public Int32 Value { get { return Left.Value - Right.Value; } }

		/// <summary>Gets the (potentially) complexity of the node.</summary>
		public Double Complexity { get { return (Left.Complexity + Right.Complexity) * 1.5; } }

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate()
		{
			// (a - b) => (b + -a).
			return new AddNode(Right.Simplify(), Left.Negate());
		}

		/// <summary>Simplifies the node.</summary>
		public IKryptoNode Simplify() 
		{
			// (a - b) => (a + -b).
			return new AddNode(Left.Simplify(), Right.Negate());
		}

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString() { return String.Format("({0} - {1})", Left, Right); }

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Gets a code for hashing purposes.</summary>
		public override int GetHashCode() 
		{
			var l= Left.GetHashCode();
			var r = Right.GetHashCode();
			return l ^ ((r << 16) | r >> 16);
		}
	}
}
