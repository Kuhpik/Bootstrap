using System;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    public static class FramerokExtensions
    {
        public static bool TryGetComponent<T>(this GameObject @object, out T component) where T : class
        {
            var goComponent = @object.GetComponent<T>();
            component = goComponent;

            return goComponent == null;
        }

        public static string GetName<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }

        public static string[] GetNames<T>(this T[] values) where T : Enum
        {
            return values.Select(x => Enum.GetName(typeof(T), x)).ToArray();
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