using System;
using System.Collections.Generic;

namespace Permutations_generation;

[Explicit]
internal class Permutations_generation
{
    [TestCase(0, 1, 2)]
    [TestCase(0, 1, 2, 3)]
    [TestCase(0, 1, 2, 3, 4)]
    public void Generates(params int[] order)
    {
        foreach (var p in order.Permutations())
        {
            Console.WriteLine($"[{string.Join(", ", p)}],");
        }

        Assert.Inconclusive();
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
