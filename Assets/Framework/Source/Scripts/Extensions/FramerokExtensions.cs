using System.Collections.Generic;
using UnityEngine;
using System;

namespace Kuhpik
{
    public static class FramerokExtensions
    {
        [Obsolete]
        public static bool TryGetComponent<T>(this GameObject @object, out T component) where T : class
        {
            var goComponent = @object.GetComponent<T>();
            component = goComponent;

            return goComponent == null;
        }

        [Obsolete]
        public static void TryAddSystem(this Dictionary<Type, IGameSystem> dictionary, GameObject @object, ref List<Type> indexes, ref List<IRunning> runnings)
        {
            if (@object.TryGetComponent<IGameSystem>(out var component))
            {
                dictionary.Add(component.GetType(), component);
                indexes.Add(component.GetType());

                if (component is IRunning)
                    runnings.Add(component as IRunning);
            }
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