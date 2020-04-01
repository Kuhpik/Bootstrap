using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Kuhpik
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds an item to the List<T>. When the sheet reaches its maximum size, it shifts the List<T> by 1 to the beginning.
        /// </summary>
        public static void Push<T>(this List<T> collection, T item, int maxCount)
        {
            if (collection.Count < maxCount)
            {
                collection.Add(item);
            }
            else
            {
                for (int i = 1; i < maxCount; i++)
                {
                    var element = collection[i];
                    collection[i - 1] = element;
                }

                collection[maxCount - 1] = item;
            }
        }

        /// <summary>
        /// Adds an element to the array at the specified index and increases the index by 1. When the array reaches its maximum size, it shifts the array by 1 to the beginning.
        /// </summary>
        public static void Push<T>(this T[] collection, ref int index, T item)
        {
            if (index < collection.Length)
            {
                collection[index] = item;
                index++;
            }
            else
            {
                for (int i = 1; i < collection.Length; i++)
                {
                    var element = collection[i];
                    collection[i - 1] = element;
                }

                collection[collection.Length - 1] = item;
            }
        }

        /// <summary>
        /// Select random element of array.
        /// </summary>
        public static T GetRandom<T>(this T[] collection)
        {
            var rng = Random.Range(0, collection.Length);
            return collection[rng];
        }

        /// <summary>
        /// Select random element of list
        /// </summary>
        public static T GetRandom<T>(this List<T> collection)
        {
            var rng = Random.Range(0, collection.Count);
            return collection[rng];
        }
    }
}