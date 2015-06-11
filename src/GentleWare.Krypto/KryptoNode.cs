using System;

namespace GentleWare.Krypto
{
	public static class KryptoNode
	{
		public static IKryptoNode Create(OperatorType opr, params IKryptoNode[] nodes)
		{
			IKryptoNode node;
			switch (opr)
			{
				case OperatorType.Add:
					node = new AddNode(nodes); break;

				case OperatorType.Subtract:
					node = new SubtractNode(nodes[0], nodes[1]);
					break;

				case OperatorType.Multiply:
					node = new MultiplyNode(nodes);
					break;

				case OperatorType.Divide:
					node = new DivideNode(nodes[0], nodes[1]);
					break;

				default: throw new NotSupportedException(opr.ToString());

			}

			return node;
		}

		/// <summary>Creates a negated node if needed, otherwise return the node it self.</summary>
		public static IKryptoNode Negate(IKryptoNode node, bool negate)
		{
			if (negate) { return new NegationNode(node); }
			return node;
		}

		/// <summary>Simplifies the node or negates it.</summary>
		public static IKryptoNode Simplify(IKryptoNode node, bool negate)
		{
			return negate ? node.Negate() : node.Simplify();
		}
	}
}
