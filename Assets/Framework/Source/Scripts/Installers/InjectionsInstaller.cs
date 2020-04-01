using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Kuhpik
{
    public class InjectionsInstaller : MonoBehaviour
    {
        [SerializeField] [ReorderableList] private ScriptableObject[] additionalInjections;

        public void Inject(IEnumerable<GameSystem> systems, params object[] injections)
        {
            if (injections == null || injections.Length == 0 || systems == null) return;

            Process(systems, injections);
            Process(systems, additionalInjections);
        }

        private void Process(IEnumerable<GameSystem> systems, params object[] injections)
        {
            foreach (var system in systems)
            {
                var type = system.GetType();
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    foreach (var @object in injections)
                    {
                        if (field.FieldType.IsAssignableFrom(@object.GetType()))
                        {
                            field.SetValue(system, @object);
                        }
                    }
                }
            }
        }
    }
}