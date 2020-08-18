using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kuhpik
{
    public static class FramerokExtensions
    {
        public static void FindOnScene<T>(this T field) where T : MonoBehaviour
        {
            field = GameObject.FindObjectOfType<T>();
        }

        public static string GetName<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }

        public static string[] GetNames<T>(this T[] values) where T : Enum
        {
            return values.Select(x => Enum.GetName(typeof(T), x)).ToArray();
        }

        public static float FromMinMax(this Vector2 minmax)
        {
            return Random.Range(minmax.x, minmax.y);
        }

        public static void PerformAction<T>(this IGameSystem system) where T : IGameSystem
        {
            if (system is T)
            {
                if (typeof(T) == typeof(IIniting))
                {
                    ((IIniting)system).OnInit();
                }

                else if (typeof(T) == typeof(IDisposing))
                {
                    ((IDisposing)system).OnDispose();
                }
            }
        }
    }
}