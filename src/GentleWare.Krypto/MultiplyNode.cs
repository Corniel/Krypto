using Qowaiv;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GentleWare.Krypto
{
	/// <summary>Represents a multiply node of the form (a * b * ...).</summary>
	public struct MultiplyNode : IKryptoNode
	{
		private static readonly NodeComparer Comparer = new NodeComparer();

		/// <summary>Underlying nodes.</summary>
		private IKryptoNode[] Arguments;

		/// <summary>Creates a new instance of the node.</summary>
		public MultiplyNode(params int[] arguments)
			: this(ValueNode.Create(arguments)) { }

		/// <summary>Creates a new instance of the node.</summary>
		public MultiplyNode(params IKryptoNode[] arguments)
		{
			Arguments = Guard.NotNullOrEmpty(arguments, "arguments");
		}

		/// <summary>Gets the Int32 value of the node.</summary>
		public Int32 Value
		{
			get
			{
				var val = 1;
				foreach (var arg in Arguments)
				{
					val *= arg.Value;
				}
				return val;
			}
		}


		/// <summary>Gets the (potentially) complexity of the node.</summary>
		public Double Complexity { get { return Arguments.Sum(node => node.Complexity) * 1.25; } }

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate()
		{
			return Simplify(Value > 0);
		}

		/// <summary>Simplifies the node.</summary>
		public IKryptoNode Simplify()
		{
			return Simplify(Value < 0);
		}

		private IKryptoNode Simplify(bool negate)
		{
			if (Arguments.Length == 1) { return KryptoNode.Simplify(Arguments[0], negate); }

			var args = new List<IKryptoNode>();

			foreach (var arg in Arguments)
			{
				var simple = KryptoNode.Simplify(arg, arg.Value < 0);

				if (simple is MultiplyNode)
				{
					args.AddRange(((MultiplyNode)simple).Arguments);
				}
				else
				{
					args.Add(simple);
				}
			}

			var twos = args.Where(arg => arg.Value == 2).OrderByDescending(arg => arg.Complexity).ToArray();
			// (2 * 2) => (2 + 2);
			if (twos.Length > 1)
			{
				var two = new AddNode(twos[0], twos[1]);
				var other = args.Where(arg => arg.Value != 2).ToList();
				other.AddRange(twos.Skip(2));

				if (other.Count == 0)
				{
					return KryptoNode.Negate(two, negate);
				}
				other.Add(two);
				return KryptoNode.Negate(new MultiplyNode(other.ToArray()), negate);
			}

			var ones = args.Where(arg => arg.Value == 1).ToArray();
			
			// (a * 1 * 1 * ...) => a * (1 * 1 * ...)
			if (ones.Length > 0 && Arguments.Length != ones.Length)
			{
				var other = args.Where(arg => arg.Value != 1).ToArray();
				var child = KryptoNode.Negate(new MultiplyNode(other), negate);
				return new MultiplyWithOneNode(child, ones);
			}

			var all = new MultiplyNode(args.OrderBy(arg => arg, Comparer).ToArray());
			return KryptoNode.Negate(all, negate);
		}

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString()
		{
			var str = String.Join(" * ", Arguments.Select(node => node.ToString()));
			return '(' + str + ')';
		}

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj)
		{
			if (obj is MultiplyNode)
			{
				var other = (MultiplyNode)obj;

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
				hash ^= Arguments[i].GetHashCode() << i;
			}
			return hash;
		}

		private class NodeComparer : IComparer<IKryptoNode>
		{
			public int Compare(IKryptoNode x, IKryptoNode y)
			{
				var compare = x.Value.CompareTo(y.Value);
				if (compare != 0) { return compare; }
				return x.Complexity.CompareTo(y.Complexity);
			}
		}
	}
}

