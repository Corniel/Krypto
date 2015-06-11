using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GentleWare.Krypto.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Input: solution n0 n1 (n2) (n3) (n4)");
                Console.Write("Input: ");
                args = Console.ReadLine().Split(new char[]{ ' '}, StringSplitOptions.RemoveEmptyEntries);

                if (args.Length > 2 && args.Length <= 6)
                {
                    var numbers = new int[args.Length - 1];
                    try
                    {
                        var sol = int.Parse(args[0]);

                        for (int i = 1; i < args.Length; i++)
                        {
                            numbers[i - 1] = int.Parse(args[i]);
                        }
                        var solutions = KryptoSolver.Solve(sol, numbers).ToList();
                        if (solutions.Count == 0)
                        {
                            Console.WriteLine("Solution:");
                            Console.WriteLine("There are no solutions.");
                            Console.WriteLine();
                        }
                        else
                        {
                            foreach (var solution in solutions)
                            {
                                Console.WriteLine(solution);
                            }
                            Console.WriteLine("Total {0} solutions.", solutions.Count);
                            Console.WriteLine();
                        }
                    }
                    catch (Exception x)
                    {
                        Console.Write("Invalid input: {0}", x.Message);
                    }
                }
                else if (args.Length == 1 && args[0] == "all")
                {
                    var nosolution = 0;
                    var all = GetCombinations(@".\Data\KryptoAllGames.txt");
                    using (var stream = new FileStream("NoSolution.txt", FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        var writer = new StreamWriter(stream);
                        foreach (var combi in all)
                        {
                            var solutions = KryptoSolver.Solve(combi.Last(), combi.Take(5).ToArray()).ToList();

                            if (solutions.Count == 0)
                            {
                                nosolution++;
                                writer.Write(combi[0]);
                                writer.Write(", ");
                                writer.Write(combi[1]);
                                writer.Write(", ");
                                writer.Write(combi[2]);
                                writer.Write(", ");
                                writer.Write(combi[3]);
                                writer.Write(", ");
                                writer.Write(combi[4]);
                                writer.Write(", ");
                                writer.WriteLine(combi[5]);
                                writer.Flush();
                                Console.Write(".");
                            }
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("No solution for {0} combinations.", nosolution);
                }
                else if (args.Length == 1 && args[0] == "no")
                {
                    var nosolution = 0;
                    var all = GetCombinations(@".\Data\KryptoZonderOplossingen.txt");
                    using (var stream = new FileStream("NoSolution2.txt", FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        var writer = new StreamWriter(stream);
                        foreach (var combi in all)
                        {
                            var solutions = KryptoSolver.Solve(combi.Last(), combi.Take(5).ToArray()).ToList();

                            if (solutions.Count > 0)
                            {
                                nosolution++;
                                //writer.Write(combi[0]);
                                //writer.Write(", ");
                                //writer.Write(combi[1]);
                                //writer.Write(", ");
                                //writer.Write(combi[2]);
                                //writer.Write(", ");
                                //writer.Write(combi[3]);
                                //writer.Write(", ");
                                //writer.Write(combi[4]);
                                //writer.Write(", ");
                                //writer.WriteLine(combi[5]);
                                writer.WriteLine("{0} = {1}", combi.Last(), solutions.First());
                                writer.Flush();
                                Console.Write('!');
                            }
                            else
                            {
                                Console.Write('.');
                            }
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("No solution for {0} combinations.", nosolution);
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }
        static List<int[]> GetCombinations(string url)
        {
            var combinations = new List<int[]>();
            using (var stream = new FileStream(url, FileMode.Open, FileAccess.Read))
            {
                var reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var combination = new int[6];
                    var splits = line.Split(',');

                    for (int i = 0; i < 6; i++)
                    {
                        combination[i] = Convert.ToInt32(splits[i]);
                    }
                    combinations.Add(combination);
                }
            }
            return combinations;
        }
    }
}
