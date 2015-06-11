using Qowaiv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentleWare.Krypto
{
	/// <summary>Represents a multiply node of the form (a * 1 * 1 ...)</summary>
	public struct MultiplyWithOneNode : IKryptoNode
	{
		/// <summary>Underlying node.</summary>
		internal IKryptoNode Child;

		/// <summary>Underlying node.</summary>
		internal IKryptoNode[] Ones;
		
		/// <summary>Creates a new instance of the node.</summary>
		public MultiplyWithOneNode(IKryptoNode child, params IKryptoNode[] ones)
		{
			Child = Guard.NotNull(child, "child");
			Ones = Guard.NotNullOrEmpty(ones, "ones");
		}

		/// <summary>Gets the Int32 value of the node.</summary>
		public Int32 Value { get { return Child.Value; } }
		

		/// <summary>Gets the (potentially) complexity of the node.</summary>
		public Double Complexity { get { return Child.Complexity; } }

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate()
		{
			return Simplify(true);
		}

		/// <summary>Simplifies the node.</summary>
		public IKryptoNode Simplify()
		{
			return Simplify(false);
		}

		private IKryptoNode Simplify(bool negate)
		{
			var ones = Ones.Select(one => one.Simplify()).ToArray();

			var child = KryptoNode.Simplify(Child, negate);

			return new MultiplyWithOneNode(child, ones);
		}

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString()
		{
			var ones = String.Join(" * ", Ones.Select(node => node.ToString()));
			return String.Format("{0} * {1}", Child, ones);
		}

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj)
		{
			if (obj is MultiplyWithOneNode)
			{
				var other = (MultiplyWithOneNode)obj;

				if (!Child.Equals(other.Child)) { return false; }

				if (Ones.Length == other.Ones.Length)
				{
					for (var i = 0; i < Ones.Length; i++)
					{
						if (!Ones[i].Equals(other.Ones[i]))
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>Gets a code for hashing purposes.</summary>
		public override int GetHashCode() { return Child.GetHashCode(); }
	}
}
