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
        /// Select random element of collection.
        /// </summary>
        public static T GetRandom<T>(this IList<T> collection)
        {
            return collection.GetRandom(0, 0);
        }

        /// <summary>
        /// Select random element of collection with specified range. Uses size of collection when max is not declared.
        /// </summary>
        public static T GetRandom<T>(this IList<T> collection, int min, int max = 0)
        {
            var rng = Random.Range(min, max == 0 ? collection.Count : max);
            return collection[rng];
        }

        /// <summary>
        /// Select random element of collection with specified range. Can have out param with RNG index
        /// </summary>
        public static T GetRandom<T>(this IList<T> collection, out int rng, int min = 0, int max = 0)
        {
            rng = Random.Range(min, max == 0 ? collection.Count : max);
            return collection[rng];
        }
    }
}