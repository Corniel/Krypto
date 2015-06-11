using Qowaiv;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GentleWare.Krypto
{
	/// <summary>Represents a solution node of the form value = solution.</summary>
	public class SolutionNode : IKryptoNode
	{
		/// <summary>Underlying node.</summary>
		private IKryptoNode Child;

		/// <summary>Creates a new instance of the node.</summary>
		public SolutionNode(IKryptoNode node) { Child = Guard.NotNull(node, "node"); }

		/// <summary>Gets the Int32 value of the node.</summary>
		public Int32 Value { get { return Child.Value; } }

		/// <summary>Gets the (potentially) complexity of the node.</summary>
		public Double Complexity { get { return Child.Complexity; } }

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate() { throw new NotSupportedException(); }

		/// <summary>Simplifies the node.</summary>
		public IKryptoNode Simplify() 
		{
			var child = Child.Simplify();
			return new SolutionNode(child); 
		}

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(Value).Append(" = ");

			var solution = Child.ToString();
			if (solution.First() == '(' && solution.Last() == ')')
			{
				solution = solution.Substring(1, solution.Length - 2);
			}
			sb.Append(solution);
			
			return sb.ToString();
		}

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj) 
		{
			var other = obj as SolutionNode;

			return other != null && other.Child.Equals(Child);
		}

		/// <summary>Gets a code for hashing purposes.</summary>
		public override int GetHashCode() { return Child.GetHashCode(); }
	}
}
