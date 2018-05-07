using System;
using System.Linq;
using NUnit.Framework;

namespace GentleWare.Krypto.UnitTests
{
    [TestFixture]
    public class KryptoSolverTest
    {
        #region No solutions

        [Test]
        public void GetSolutions_1c1a3_NoSolutions()
        {
            var solutions = KryptoSolver.GetSolutions(3, 1, 1);
            SolutionsAssert.NoSolutions(solutions);
        }

        [Test]
        public void GetSolutions_1c1c1a14_NoSolutions()
        {
            var solutions = KryptoSolver.GetSolutions(14, 1, 1, 1);
            SolutionsAssert.NoSolutions(solutions);
        }

        [Test]
        public void GetSolutions_c1c1c1c2c2a25_NoSolutions()
        {
            var solutions = KryptoSolver.GetSolutions(25, 1, 1, 1, 2, 2);
            SolutionsAssert.NoSolutions(solutions);
        }
        [Test]
        public void Solve_c3c8c14c16c17a22_NoSolutions()
        {
            var solutions = KryptoSolver.Solve(14, 3, 8, 16, 17, 22);
            SolutionsAssert.NoSolutions(solutions);
        }

        #endregion

        #region Solve with QueryString

        [Test]
        public void Solve_StringNull_NoSolutions()
        {
            var solutions = KryptoSolver.Solve((String)null);
            SolutionsAssert.NoOutcome(solutions);
        }

        [Test]
        public void Solve_StringEmpty_NoSolutions()
        {
            var solutions = KryptoSolver.Solve(String.Empty);
            SolutionsAssert.NoOutcome(solutions);
        }

        [Test]
        public void Solve_nonsense_NoOutcome()
        {
            var solutions = KryptoSolver.Solve("nonsense");
            SolutionsAssert.NoOutcome(solutions);
        }

        [Test]
        public void Solve_4c3_NoSolutions()
        {
            var solutions = KryptoSolver.Solve("4,3");
            SolutionsAssert.NoOutcome(solutions);
        }
        [Test]
        public void Solve_4c3cnonesensec7_OneSolution()
        {
            var act = KryptoSolver.Solve("4,3,nonsense,7").ToArray();
            var exp = new SolutionNode[]
            {
                new SolutionNode(new AddNode(7, -3)),
            };
            CollectionAssert.AreEqual(exp, act);
        }
        [Test]
        public void Solve_1c2c3c4c5c6c7_NoOutcome()
        {
            var solutions = KryptoSolver.Solve("1,2,3,4,5,6,7");
            SolutionsAssert.NoOutcome(solutions);
        }

        #endregion

        #region 2 Cards

        [Test]
        public void GetSolutions_3c2a6_2x3()
        {
            var solutions = KryptoSolver.GetSolutions(6, 3, 2);
            SolutionsAssert.HasSolutions(6, solutions, "6 = 2 * 3");
        }

        [Test]
        public void Solve_3c2a6_2x3()
        {
            var solutions = KryptoSolver.Solve(6, 3, 2);
            SolutionsAssert.HasSolutions(6, solutions, "6 = 2 * 3");
        }

        [Test]
        public void Solve_c2c2a4_2p2()
        {
            var solutions = KryptoSolver.Solve(4, 2, 2);
            SolutionsAssert.HasSolutions(4, solutions, "4 = 2 + 2");
        }

        [Test]
        public void Solve_c6c23a17_23m6()
        {
            var solutions = KryptoSolver.Solve(17, 6, 23);
            SolutionsAssert.HasSolutions(17, solutions, "17 = 23 - 6");
        }

        [Test]
        public void Solve_c17c2c3a11_17m2x3()
        {
            var solutions = KryptoSolver.Solve(11, 17, 2, 3).ToList();
            SolutionsAssert.HasSolutions(11, solutions, "11 = 17 - (2 * 3)");
        }

        #endregion

        #region 3 Cards

        [Test]
        public void Solve_2c1c1a4_1p1p2()
        {
            var solutions = KryptoSolver.Solve(4, 2, 1, 1);
            SolutionsAssert.HasSolutions(4, solutions, "4 = 1 + 1 + 2");
        }

        [Test]
        public void Solve_2c3c5a11_5p2x3()
        {
            var solutions = KryptoSolver.Solve(11, 2, 3, 5);
            SolutionsAssert.HasSolutions(11, solutions, "11 = 5 + (2 * 3)");
        }

        #endregion

        #region 4 Cards

        [Test]
        public void Solve_c2c2c2c2_a16_2p2x2p2()
        {
            var solutions = KryptoSolver.Solve(16, 2, 2, 2, 2);
            SolutionsAssert.HasSolutions(16, solutions, "16 = (2 + 2) * (2 + 2)");
        }

        [Test, Ignore("?")]
        public void Solve_c3c3c3c7c17_a24_1Solutions()
        {
            var solutions = KryptoSolver.Solve(24, 3, 3, 7, 17);
            SolutionsAssert.HasSolutions(24, solutions, "24 = 7 + 17 + (3 - 3)");
        }

        #endregion

        #region 5 Cards

        [Test]
        public void Solve_c1c7c9c13c16a16_3Solutions()
        {
            var solutions = KryptoSolver.Solve(16, 1, 7, 9, 13, 16).ToList();
            SolutionsAssert.HasSolutions(16, solutions,
                "16 = ((13 - 9) * (1 + 7)) - 16",
                "16 = (((13 * 16) - 1) / 9) - 7",
                "16 = (1 + (9 * (7 + 16))) / 13");
        }

        [Test, Ignore("?")]
        public void Solve_c1c2c3c4c5a25_4Solutions()
        {
            var solutions = KryptoSolver.Solve(25, 1, 2, 3, 4, 5);
            SolutionsAssert.HasSolutions(25, solutions, "25 = 5 * (1 + 4) * (3 - 2)");
        }

        #endregion

        #region 5 Cards

        [Test, Ignore("?")]
        public void Solve_c3c8c11c16c25a18_1Solutions()
        {
            var solutions = KryptoSolver.GetSolutions(18, 3, 8, 11, 16, 25).ToList();

            Assert.AreEqual(9, solutions.Count);
        }




        [Test, Ignore("?")]
        public void Solve_c4c2c1_8Solutions()
        {
            var solutions = KryptoSolver.GetSolutions(2, 4, 2, 1).ToList();

            Assert.AreEqual(6, solutions.Count, "solutions.Count");

            solutions = KryptoSolver.Simplify(solutions).ToList();

            Assert.AreEqual(1, solutions.Count, "solutions.Count Simplified");
            Assert.AreEqual("2 = (4 - 2) * 1", solutions[0].ToString(), "solutions[0]");
        }

        [Test, Ignore("?")]
        public void Solve_c4c4c5c13c13a11_8Solutions()
        {
            var solutions = KryptoSolver.Solve(11, 4, 4, 5, 13, 13).ToList();

            Assert.AreEqual(11, solutions.Count, "solutions.Count");

            solutions = KryptoSolver.Simplify(solutions).ToList();

            Assert.IsTrue(solutions.All(s => s.Value == 11), "All are 11");
            Assert.AreEqual(4, solutions.Count, "solutions.Count Simplified");
        }



        [Test, Ignore("?")]
        public void Solve_c3c4c5c8c21a17_8Solutions()
        {
            var solutions = KryptoSolver.Solve(17, 3, 4, 5, 8, 21).ToList();

            Assert.AreEqual(278, solutions.Count, "solutions.Count");

            solutions = KryptoSolver.Simplify(solutions).ToList();
            Assert.IsTrue(solutions.All(s => s.Value == 17), "All are 17");
            Assert.AreEqual(4, solutions.Count, "solutions.Count Simplified");
        }

        [Test, Ignore("?")]
        public void Solve_c1c2c7c8c11a9_10Solutions()
        {
            var solutions = KryptoSolver.Solve(9, 1, 2, 7, 8, 11).ToList();

            Assert.AreEqual(29, solutions.Count, "solutions.Count");

            solutions = KryptoSolver.Simplify(solutions).ToList();
            Assert.IsTrue(solutions.All(s => s.Value == 9), "All are 9");

            Assert.AreEqual(10, solutions.Count, "solutions.Count Simplified");
        }

        [Test, Ignore("?")]
        public void Solve_c2c4c7c8c17a11_1Solutions()
        {
            var solutions = KryptoSolver.GetSolutions(11, 2, 4, 7, 8, 17).ToList();

            Assert.AreEqual(115, solutions.Count, "solutions.Count");

            solutions = KryptoSolver.Simplify(solutions).ToList();
            Assert.IsTrue(solutions.All(s => s.Value == 11), "All are 11");
            Assert.AreEqual(1, solutions.Count, "solutions.Count Simplified");
        }
        #endregion
    }
}
