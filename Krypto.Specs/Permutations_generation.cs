using System;
using System.Collections.Generic;
using System.Linq;

namespace Permutations_generation;

internal class Permutations_generation
{
    [TestCase(006, /* => */ 0, 1, 2)]
    [TestCase(024, /* => */ 0, 1, 2, 3)]
    [TestCase(010, /* => */ 0, 0, 1, 1, 1)]
    [TestCase(020, /* => */ 0, 1, 2, 2, 2)]
    [TestCase(030, /* => */ 0, 1, 1, 2, 2)]
    [TestCase(060, /* => */ 0, 1, 2, 3, 3)]
    [TestCase(120, /* => */ 0, 1, 2, 3, 4)]
    public void Generates(int count, params int[] order)
    {
        var lines = new HashSet<string>();

        foreach (var p in order.Permutations())
            lines.Add($"[{string.Join(", ", p)}],");
        
        foreach(var line in lines.Order())
            Console.WriteLine(line);

        lines.Should().HaveCount(count);
    }
}

internal static class HeapPermutations
{
    public static IEnumerable<T[]> Permutations<T>(this T[] values)
           => values.Permutations(values.Length, 0);

    /// <remarks>Heap's algorithm.</remarks>
    private static IEnumerable<T[]> Permutations<T>(this T[] array, int size, int n)
    {
        if (size == 1)
        {
            yield return array.Copy();
        }

        for (int i = 0; i < size; i++)
        {
            foreach (var result in Permutations(array, size - 1, n))
            {
                yield return result;
            }

            array.Swap((size % 2 == 0) ? i : 0, size - 1);
        }
    }

    private static void Swap<T>(this T[] array, int index0, int index1)
    {
        (array[index1], array[index0]) = (array[index0], array[index1]);
    }

    private static T[] Copy<T>(this T[] array)
    {
        var copy = new T[array.Length];
        Array.Copy(array, copy, array.Length);
        return copy;
    }
}
