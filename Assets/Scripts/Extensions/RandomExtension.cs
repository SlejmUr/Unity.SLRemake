using System.Collections.Generic;
using UnityEngine;

namespace SLRemake.Extensions
{
    public static class RandomExtension
    {
        public static T RandomItem<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static T RandomItem<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static int RandomIndex<T>(this T[] array)
        {
            return Random.Range(0, array.Length);
        }

        public static int RandomIndex<T>(this IList<T> list)
        {
            return Random.Range(0, list.Count);
        }

        public static bool TryGetRandomItem<T>(this IList<T> list, out T random)
        {
            if (list.Count > 0)
            {
                random = list.RandomItem();
                return true;
            }
            random = default;
            return false;
        }

        public static T PullRandomItem<T>(this IList<T> list)
        {
            int index = Random.Range(0, list.Count);
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }
    }

}