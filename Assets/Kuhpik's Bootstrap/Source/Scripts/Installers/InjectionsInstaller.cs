﻿using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Kuhpik
{
    [DefaultExecutionOrder(100)]
    public class InjectionsInstaller : MonoBehaviour
    {
        [SerializeField] [ReorderableList] ScriptableObject[] additionalInjections;

        void Start()
        {
            Bootstrap.OnGamePreStartEvent += Process;
        }

        void Process()
        {
            Inject(Bootstrap.systems.Values, Bootstrap.itemsToInject);
        }

        void Inject(IEnumerable<GameSystem> systems, params object[] injections)
        {
            if (injections == null || injections.Length == 0 || systems == null) return;

            Process(systems, injections.Concat(additionalInjections).ToArray());
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