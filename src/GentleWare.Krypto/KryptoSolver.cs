using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GentleWare.Krypto
{
    /// <summary>A solver for Krypto.</summary>
    /// <remarks>
    /// Less the two or more then five numbers to solve the Krypto are not
    /// supported.
    /// </remarks>
    public static class KryptoSolver
    {
        private static readonly char[] Splitter = ";,. ".ToArray();



        /// <summary>Solves a Krypto puzzle based on a query string.</summary>
        /// <param name="query">
        /// The query string to parse.
        /// </param>
        /// <remarks>
        /// The query string is assumed to be list of numbers concatenate by a
        /// semicolon, comma, dot, or space. invalid elements are ignored. If
        /// 
        /// The first (valid) number is the solution to search for, the other
        /// (valid) numbers are seen as the numbers for the solution.
        /// </remarks>
        public static IEnumerable<SolutionNode> Solve(string query)
        {
            var args = (query ?? String.Empty).Split(Splitter, StringSplitOptions.RemoveEmptyEntries);

            var numbers = args.Select(arg =>
            {
                if (Int32.TryParse(arg, out int n)) { return n; }
                return SolutionNode.NoSolutionValue;
            }).Where(n => n != SolutionNode.NoSolutionValue).ToList();

            if (numbers.Count > 2 && numbers.Count < 7)
            {
                var solution = numbers[0];
                return Solve(solution, numbers.Skip(1).ToArray());
            }
            else
            {
                return Enumerable.Empty<SolutionNode>();
            }
        }

        public static IEnumerable<SolutionNode> Solve(int solution, params int[] numbers)
        {
            return Simplify(GetSolutions(solution, numbers));
        }

        public static IEnumerable<SolutionNode> GetSolutions(int solution, params int[] numbers)
        {
            if (numbers == null || numbers.Length < 2) { throw new ArgumentException("At least two numbers should be specified.", "numbers"); }

            IEnumerable<SolutionNode> solutions;

            switch (numbers.Length)
            {
                case 5: solutions = Solve5(solution, numbers).Distinct().Select(n => new SolutionNode(n)); break;
                case 4: solutions = Solve4(solution, numbers).Distinct().Select(n => new SolutionNode(n)); break;
                case 3: solutions = Solve3(solution, numbers).Distinct().Select(n => new SolutionNode(n)); break;
                case 2: solutions = Solve2(solution, numbers).Distinct().Select(n => new SolutionNode(n)); break;
                default: throw new NotSupportedException("More then five cards is not supported yet.");
            }
            if (solutions.Any())
            {
                return solutions;
            }
            return new SolutionNode[] { SolutionNode.None };
        }

        public static IEnumerable<SolutionNode> Simplify(IEnumerable<SolutionNode> nodes)
        {
            var distincts = new HashSet<SolutionNode>();
            var simplifiers = new HashSet<IKryptoNode>();

            foreach (var node in nodes)
            {
                simplifiers.Clear();

                IKryptoNode cur = node;
                do
                {
                    cur = cur.Simplify();
                    Debug.Assert(node.Value == cur.Value, "Value changed.");

                }
                while (simplifiers.Add(cur));

                var best = simplifiers.OrderBy(s => s.Complexity).FirstOrDefault();

                distincts.Add((SolutionNode)best);
            }
            return distincts.OrderBy(s => s.Complexity);
        }

        #region 2 Cards

        private static IEnumerable<IKryptoNode> Solve2(int solution, params int[] numbers)
        {
            var orders = new List<List<int>>()
            {
                new List<int>(){ numbers[0], numbers[1] },
            };
            if (numbers[0] != numbers[1])
            {
                orders.Add(new List<int>() { numbers[1], numbers[0] });
            }

            foreach (var order in orders)
            {
                foreach (var opr in Operators.All)
                {
                    var res = Operate(order[0], order[1], opr);

                    if (res != SolutionNode.NoSolutionValue && solution == res)
                    {
                        yield return KryptoNode.Create(opr, new ValueNode(order[0]), new ValueNode(order[1]));
                    }
                }
            }
        }

        #endregion

        #region 3 Cards

        private static IEnumerable<IKryptoNode> Solve3(int solution, params int[] numbers)
        {
            // we need two bit for the four different operators.
            var operators = 16; // ((numbers.Length - 1) << 4) << 2

            var numberOrder = CreateNumberOrderList(new List<int>(), 0, 0, numbers.Length);

            while (numberOrder != null)
            {
                // Loop all operator mutations.
                for (int i_operator = 0; i_operator < operators; i_operator++)
                {
                    var c0 = numbers[numberOrder[0]];
                    var c1 = numbers[numberOrder[1]];
                    var c2 = numbers[numberOrder[2]];

                    var node3_tree0 = New3CardsTree0(solution, i_operator, c0, c1, c2);
                    var node3_tree1 = New3CardsTree1(solution, i_operator, c0, c1, c2);

                    if (node3_tree0 != null)
                    {
                        yield return node3_tree0;
                    }
                    if (node3_tree1 != null)
                    {
                        yield return node3_tree1;
                    }
                }
                // Go back at least two positions.
                numberOrder = CreateNumberOrderList(numberOrder, -1, numbers.Length - 2, numbers.Length);
            }
        }

        // 3 numbers
        //     o     
        //    / \   
        //   o   1  
        //  / \
        // 1   1
        private static IKryptoNode New3CardsTree0(int solution, int operators, int c0, int c1, int c2)
        {
            //       o0     
            //      /  \   
            //    o1    c0 
            //   / \ 
            // c1   c2

            var o1 = GetOperatorType(operators, 1);
            var n1 = Operate(c1, c2, o1);
            if (n1 < 0) { return null; }

            var o0 = GetOperatorType(operators, 0);
            var n0 = Operate(n1, c0, o0);
            if (n0 < 0 || n0 != solution) { return null; }

            // create new Solution.
            return
            KryptoNode.Create
            (
                o0,
                KryptoNode.Create
                (
                    o1,
                    new ValueNode(c1),
                    new ValueNode(c2)
                ),
                new ValueNode(c0)
            );
        }

        // 3 numbers
        //   o     
        //  / \   
        // 1   o
        //    / \
        //   1   1
        private static IKryptoNode New3CardsTree1(int solution, int operators, int c0, int c1, int c2)
        {
            //   o0     
            //  /  \   
            //o1    c0 
            //     / \ 
            //   c1   c2

            var o1 = GetOperatorType(operators, 1);
            var n1 = Operate(c1, c2, o1);
            if (n1 < 0) { return null; }

            var o0 = GetOperatorType(operators, 0);
            var n0 = Operate(c0, n1, o0);
            if (n0 < 0 || n0 != solution) { return null; }

            // create new Solution.
            return
            KryptoNode.Create
            (
                o0,
                new ValueNode(c0),
                KryptoNode.Create
                (
                    o1,
                    new ValueNode(c1),
                    new ValueNode(c2)
                )
            );
        }

        #endregion

        #region 4 Cards

        private static IEnumerable<IKryptoNode> Solve4(int solution, params int[] numbers)
        {
            // we need two bit for the four different operators.
            var operators = 64; // ((numbers.Length - 1) << 4) << 2

            var numberOrder = CreateNumberOrderList(new List<int>(), 0, 0, numbers.Length);

            while (numberOrder != null)
            {
                // Loop all operator mutations.
                for (int i_operator = 0; i_operator < operators; i_operator++)
                {
                    var c0 = numbers[numberOrder[0]];
                    var c1 = numbers[numberOrder[1]];
                    var c2 = numbers[numberOrder[2]];
                    var c3 = numbers[numberOrder[3]];

                    var node4_tree0 = New4CardsTree0(solution, i_operator, c0, c1, c2, c3);
                    var node4_tree1 = New4CardsTree1(solution, i_operator, c0, c1, c2, c3);

                    if (node4_tree0 != null)
                    {
                        yield return node4_tree0;
                    }
                    if (node4_tree1 != null)
                    {
                        yield return node4_tree1;
                    }
                }
                // Go back at least two positions.
                numberOrder = CreateNumberOrderList(numberOrder, -1, numbers.Length - 2, numbers.Length);
            }
        }

        // 4 numbers
        //     o     
        //   /   \   
        //  o     o  
        // / \   / \ 
        // 1 1   1 1 
        private static IKryptoNode New4CardsTree0(int solution, int operators, int c0, int c1, int c2, int c3)
        {
            //       o0     
            //      /  \   
            //    o1    o2 
            //   / \   /  \ 
            // c0  c1 c2   c3 

            var o1 = GetOperatorType(operators, 1);
            var n1 = Operate(c0, c1, o1);
            if (n1 < 0) { return null; }

            var o2 = GetOperatorType(operators, 2);
            var n2 = Operate(c2, c3, o2);
            if (n2 < 0) { return null; }

            var o0 = GetOperatorType(operators, 0);
            var n0 = Operate(n1, n2, o0);
            if (n0 < 0 || n0 != solution) { return null; }

            // create new Solution.
            return
            KryptoNode.Create
            (
                o0,
                KryptoNode.Create
                (
                    o1,
                    new ValueNode(c0),
                    new ValueNode(c1)
                ),
                KryptoNode.Create
                (
                    o2,
                    new ValueNode(c2),
                    new ValueNode(c3)
                )
            );
        }
        // 4 numbers
        //              o
        //             / \
        //            o   1
        //           / \
        //          o   1
        //         / \
        //        1   1
        public static IKryptoNode New4CardsTree1(int solution, int operators, int c0, int c1, int c2, int c3)
        {
            //             o0
            //            /  \
            //          o1    c0
            //         /  \
            //       o2    c1
            //      /  \
            //    c2    c3

            var o2 = GetOperatorType(operators, 2);
            var n2 = Operate(c2, c3, o2);
            if (n2 < 0) { return null; }

            var o1 = GetOperatorType(operators, 1);
            var n1 = Operate(n2, c1, o1);
            if (n1 < 0) { return null; }

            var o0 = GetOperatorType(operators, 0);
            var n0 = Operate(n1, c0, o0);
            if (n0 < 0 || n0 != solution) { return null; }

            // create new Solution.
            var node =
            KryptoNode.Create
            (
                o0,
                KryptoNode.Create
                (
                    o1,
                    KryptoNode.Create
                    (
                        o2,
                        new ValueNode(c2),
                        new ValueNode(c3)
                    ),
                    new ValueNode(c1)
                ),
                new ValueNode(c0)
            );
            return node;
        }

        #endregion

        #region 5 Cards

        private static IEnumerable<IKryptoNode> Solve5(int solution, params int[] numbers)
        {
            // we need two bit for the four different operators.
            var operators = 256; // ((numbers.Length - 1) << 4) << 2

            var numberOrder = CreateNumberOrderList(new List<int>(), 0, 0, numbers.Length);

            while (numberOrder != null)
            {
                // Loop all operator mutations.
                for (int i_operator = 0; i_operator < operators; i_operator++)
                {
                    var c0 = numbers[numberOrder[0]];
                    var c1 = numbers[numberOrder[1]];
                    var c2 = numbers[numberOrder[2]];
                    var c3 = numbers[numberOrder[3]];
                    var c4 = numbers[numberOrder[4]];

                    var node5_tree0 = New5CardsTree0(solution, i_operator, c0, c1, c2, c3, c4);
                    var node5_tree1 = New5CardsTree1(solution, i_operator, c0, c1, c2, c3, c4);
                    var node5_tree2 = New5CardsTree2(solution, i_operator, c0, c1, c2, c3, c4);

                    if (node5_tree0 != null)
                    {
                        yield return node5_tree0;
                    }
                    if (node5_tree1 != null)
                    {
                        yield return node5_tree1;
                    }
                    if (node5_tree2 != null)
                    {
                        yield return node5_tree2;
                    }
                }

                // Go back at least two positions.
                numberOrder = CreateNumberOrderList(numberOrder, -1, numbers.Length - 2, numbers.Length);
            }
        }

        // 5 numbers
        //        o     
        //      /   \   
        //     o     o  
        //    / \   / \ 
        //   o  1   1 1 
        //  / \         
        // 1   1        
        private static IKryptoNode New5CardsTree0(int solution, int operators, int c0, int c1, int c2, int c3, int c4)
        {
            var o3 = GetOperatorType(operators, 3);
            var n3 = Operate(c3, c4, o3);
            if (n3 < 0) { return null; }

            var o1 = GetOperatorType(operators, 1);
            var n1 = Operate(n3, c0, o1);
            if (n1 < 0) { return null; }

            var o2 = GetOperatorType(operators, 2);
            var n2 = Operate(c1, c2, o2);
            if (n2 < 0) { return null; }

            var o0 = GetOperatorType(operators, 0);
            var n0 = Operate(n1, n2, o0);
            if (n0 < 0 || n0 != solution) { return null; }

            // create new Solution.
            return
            KryptoNode.Create
            (
                o0,
                KryptoNode.Create
                (
                    o1,
                    KryptoNode.Create
                    (
                        o3,
                        new ValueNode(c3),
                        new ValueNode(c4)
                    ),
                    new ValueNode(c0)
                ),
                KryptoNode.Create
                (
                    o2,
                    new ValueNode(c1),
                    new ValueNode(c2)
                )
            );
        }
        // 5 numbers
        //        o   
        //       / \  
        //      o   1 
        //    /   \   
        //   o     o  
        //  / \   / \ 
        //  1 1   1 1 
        public static IKryptoNode New5CardsTree1(int solution, int operators, int c0, int c1, int c2, int c3, int c4)
        {
            var o2 = GetOperatorType(operators, 2);
            var n2 = Operate(c1, c2, o2);
            if (n2 < 0) { return null; }

            var o3 = GetOperatorType(operators, 3);
            var n3 = Operate(c3, c4, o3);
            if (n3 < 0) { return null; }

            var o1 = GetOperatorType(operators, 1);
            var n1 = Operate(n2, n3, o1);
            if (n1 < 0) { return null; }

            var o0 = GetOperatorType(operators, 0);
            var n0 = Operate(n1, c0, o0);
            if (n0 < 0 || n0 != solution) { return null; }

            // create new Solution.
            return
            KryptoNode.Create
            (
                o0,
                KryptoNode.Create
                (
                    o1,
                    KryptoNode.Create
                    (
                        o2,
                        new ValueNode(c1),
                        new ValueNode(c2)
                    ),
                    KryptoNode.Create
                    (
                        o3,
                        new ValueNode(c3),
                        new ValueNode(c4)
                    )
                ),
                new ValueNode(c0)
            );
        }
        // 5 numbers
        //        o
        //       / \
        //      o   1
        //     / \
        //    o   1
        //   / \
        //  o   1
        // / \
        //1   1
        public static IKryptoNode New5CardsTree2(int solution, int operators, int c0, int c1, int c2, int c3, int c4)
        {
            //             o0
            //            /  \
            //          o1    c0
            //         /  \
            //       o2    c1
            //      /  \
            //    o3    c2
            //   /  \
            // c3    c4

            var o3 = GetOperatorType(operators, 3);
            var n3 = Operate(c3, c4, o3);
            if (n3 < 0) { return null; }

            var o2 = GetOperatorType(operators, 2);
            var n2 = Operate(n3, c2, o2);
            if (n2 < 0) { return null; }

            var o1 = GetOperatorType(operators, 1);
            var n1 = Operate(n2, c1, o1);
            if (n1 < 0) { return null; }

            var o0 = GetOperatorType(operators, 0);
            var n0 = Operate(n1, c0, o0);
            if (n0 < 0 || n0 != solution) { return null; }

            // create new Solution.
            var node =
            KryptoNode.Create
            (
                o0,
                KryptoNode.Create
                (
                    o1,
                    KryptoNode.Create
                    (
                        o2,
                        KryptoNode.Create
                        (
                            o3,
                            new ValueNode(c3),
                            new ValueNode(c4)
                        ),
                        new ValueNode(c2)
                    ),
                    new ValueNode(c1)
                ),
                new ValueNode(c0)
            );
            return node;
        }

        #endregion

        /// <summary>Creates a ordered list.</summary>
        /// <remarks>
        /// Returns null if the last possible list is used as input.
        /// </remarks>
        public static List<int> CreateNumberOrderList(List<int> list, int index, int pos, int length)
        {
            // Check if the last possible list is given.
            if (list.Count == length)
            {
                bool isLast = true;
                for (int i = 0; i < length; i++)
                {
                    // The last possible has the form: root - 1, root - 2, ... , 1, 0
                    if (i + list[i] != length - 1)
                    {
                        isLast = false;
                        break;
                    }
                }
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                // This is the actual contract.
                if (isLast) { return null; }
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

                // No index set so determine a new one.
                else if (index == -1)
                {
                    index = list[pos] + 1;
                }
            }
            // we're falling back, shorten the list.
            if (pos < list.Count)
            {
                list = list.Take(pos).ToList();
            }

            // we're outside our boundary.
            if (index >= length)
            {
                index = list.Last() + 1;
                pos--;
                return CreateNumberOrderList(list, index, pos, length);
            }
            // Not unique, try next.
            if (list.Contains(index))
            {
                index++;
                return CreateNumberOrderList(list, index, pos, length);
            }
            // Add item.
            else
            {
                list.Add(index);
            }
            if (list.Count < length)
            {
                pos++;
                return CreateNumberOrderList(list, 0, pos, length);
            }
            return list;
        }

        public static OperatorType GetOperatorType(int operators, int index)
        {
            var opr = (OperatorType)((operators >> (index << 1)) & 3);
            return opr;
        }

        /// <summary>Applies the operator on arg0 and arg1.</summary>
        /// <returns>
        /// The outcome of the operation or SolutionNode.NoSolutionValue if not applicable.
        /// </returns>
        private static int Operate(int arg0, int arg1, OperatorType opr)
        {
            var result = 0;

            switch (opr)
            {
                case OperatorType.Add:
                    result = arg0 + arg1;
                    break;

                case OperatorType.Multiply:
                    // No 2 * 2 (use 2 + 2)
                    if (arg0 == 2 && arg1 == 2) { return SolutionNode.NoSolutionValue; }
                    result = arg0 * arg1;
                    break;

                case OperatorType.Divide:
                    // no 0 / root ==> 0 * root
                    // no root / 0 ==> NaN
                    // no root / 1 ==> root * 1
                    // and no fractions.
                    if (arg0 == 0 ||
                        arg1 == 0 ||
                        arg1 == 1 ||
                        arg0 % arg1 != 0) { return SolutionNode.NoSolutionValue; }
                    // No 4 / 2 => 4 - 2
                    if (arg0 == 4 && arg1 == 2) { return SolutionNode.NoSolutionValue; }
                    result = arg0 / arg1;
                    break;

                case OperatorType.Subtract:
                    if (arg0 < arg1) { return SolutionNode.NoSolutionValue; }
                    result = arg0 - arg1;
                    break;

                default:
                    throw new NotSupportedException(opr.ToString());
            }
            return result;
        }
    }
}
