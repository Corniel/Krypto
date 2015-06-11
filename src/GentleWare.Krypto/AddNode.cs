using Qowaiv;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GentleWare.Krypto
{
	/// <summary>Represents a add node of the form (a + b + ...).</summary>
	public struct AddNode : IKryptoNode
	{
		private static readonly NodeComparer Comparer = new NodeComparer();

		/// <summary>Underlying node.</summary>
		private IKryptoNode[] Arguments;

		/// <summary>Creates a new instance of the node.</summary>
		public AddNode(params int[] arguments)
			: this(ValueNode.Create(arguments)) { }

		/// <summary>Creates a new instance of the node.</summary>
		public AddNode(params IKryptoNode[] arguments)
		{
			Arguments = Guard.NotNullOrEmpty(arguments, "arguments");

		}

		/// <summary>Gets the Int32 value of the node.</summary>
		public Int32 Value { get { return Arguments.Sum(node => node.Value); } }

		/// <summary>Gets the (potentially) complexity of the node.</summary>
		public Double Complexity { get { return Arguments.Sum(node => node.Complexity) * 1.01; } }

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate()
		{
			return Simplify(Value != 0);
		}

		/// <summary>Simplifies the node.</summary>
		public IKryptoNode Simplify()
		{
			return Simplify(false);
		}

		private IKryptoNode Simplify(bool negate)
		{
			if (Arguments.Length == 1) { return KryptoNode.Simplify(Arguments[0], negate); }

			var args = new List<IKryptoNode>();
			var ones = new List<IKryptoNode>();

			foreach (var arg in Arguments)
			{
				var simple = KryptoNode.Simplify(arg, negate);

				if (simple is AddNode)
				{
					args.AddRange(((AddNode)simple).Arguments);
				}
				// (a + (b * 1)) == (a + b) * 1
				else if (simple is MultiplyWithOneNode)
				{
					var mwon = (MultiplyWithOneNode)simple;
					args.Add(mwon.Child);
					ones.AddRange(mwon.Ones);
				}
				else
				{
					args.Add(simple);
				}
			}

			if (ones.Count > 0)
			{
				if (args.Count == 0)
				{
					return new MultiplyNode(ones.ToArray());
				}
				return new MultiplyWithOneNode(
					new AddNode(args.OrderBy(arg => arg, Comparer).ToArray()),
					ones.ToArray());
			}

			return new AddNode(args.OrderBy(arg => arg, Comparer).ToArray());
		}

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString()
		{
			var str = String.Join(" + ", Arguments.Select(node => node.ToString()));
			str = str.Replace(" + -", " - ");
			return '(' + str + ')';
		}

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj)
		{
			if (obj is AddNode)
			{
				var other = (AddNode)obj;

				if (Arguments.Length == other.Arguments.Length)
				{
					for (var i = 0; i < Arguments.Length; i++)
					{
						if (!Arguments[i].Equals(other.Arguments[i]))
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
		public override int GetHashCode()
		{
			var hash = Arguments[0].GetHashCode();
			for (var i = 1; i < Arguments.Length; i++)
			{
				hash ^= Arguments[i].GetHashCode() << (i << 1);
			}
			return hash;
		}


		private class NodeComparer : IComparer<IKryptoNode>
		{
			public int Compare(IKryptoNode x, IKryptoNode y)
			{
				var compare = (x.Value < 0).CompareTo(y.Value < 0);
				if (compare != 0) { return compare; }

				compare = x.Value.CompareTo(y.Value);
				if (compare != 0) { return compare; }

				return x.Complexity.CompareTo(y.Complexity);
			}
		}
	}
}
