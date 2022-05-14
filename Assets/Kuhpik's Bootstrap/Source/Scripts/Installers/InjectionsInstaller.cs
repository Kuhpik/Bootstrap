using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Kuhpik
{
    public class InjectionsInstaller : Installer
    {
        [SerializeField] [ReorderableList] ScriptableObject[] additionalInjections;

        public override int Order => 100;

        public override void Process()
        {
            Bootstrap.Instance.GamePreStartEvent += Inject;
        }

        void Inject()
        {
            Inject(Bootstrap.Instance.systems.Values.ToArray(), Bootstrap.Instance.itemsToInject.Concat(additionalInjections).ToArray());
        }

        void Inject(IEnumerable<GameSystem> systems, params object[] injections)
        {
            if (injections == null || systems == null || injections.Length == 0) return;

            Process(systems, injections);
        }

        void Process(IEnumerable<GameSystem> systems, params object[] injections)
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