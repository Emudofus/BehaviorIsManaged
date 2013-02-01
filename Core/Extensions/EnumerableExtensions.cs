using System;
using System.Collections.Generic;
using System.Linq;

namespace BiM.Core.Extensions
{
  public static class EnumerableExtensions
  {

    private static Random rnd = new Random();

    /// <summary>
    /// Select a random item within the elements of an IEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputSet"></param>
    /// <returns></returns>
    static public T GetRandom<T>(this IEnumerable<T> inputSet)
    {
      return GetRandom(inputSet.ToArray());
    }

    /// <summary>
    /// Select a random item within the elements of an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputSet"></param>
    /// <returns></returns>
    static public T GetRandom<T>(this T[] inputSet)
    {
      if (inputSet.Length == 0) return default(T);
      return inputSet[rnd.Next(inputSet.Length)];
    }

  }
}
