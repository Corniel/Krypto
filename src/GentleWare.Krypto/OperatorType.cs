using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GentleWare.Krypto
{
	public enum OperatorType
	{
		Multiply = 0,
		Divide = 1,
		Add = 2,
		Subtract = 3,
	}
	public static class Operators
	{
		public static readonly ReadOnlyCollection<OperatorType> All = new ReadOnlyCollection<OperatorType>(
			new List<OperatorType>() 
			{
				OperatorType.Multiply, 
				OperatorType.Divide, 
				OperatorType.Add, 
				OperatorType.Subtract 
			});
	}
}
