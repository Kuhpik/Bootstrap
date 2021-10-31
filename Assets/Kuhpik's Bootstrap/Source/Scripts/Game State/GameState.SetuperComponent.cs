using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        internal class SetuperComponent : MonoBehaviour
        {
            [SerializeField] Identificator type;
            [SerializeField] bool useAdditionalScreens;
            [SerializeField] bool useAdditionalStates;
            [SerializeField] [ReorderableList] Identificator[] allowedTransitions;
            [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] Identificator[] additionalStatesInTheBegining;
            [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] Identificator[] additionalStatesInTheEnd;
            [SerializeField] [ReorderableList] [ShowIf("useAdditionalScreens")] Identificator[] additionalScreens;

            public Identificator ID => type;
            public bool UseAdditionalScreens => useAdditionalScreens;
            public bool UseAdditionalStates => useAdditionalStates;
            public Identificator[] AdditionalStatesInTheBegining => additionalStatesInTheBegining;
            public Identificator[] AdditionalStatesInTheEnd => additionalStatesInTheEnd;
            public Identificator[] AdditionalScreens => additionalScreens;
            public Identificator[] AllowedTransitions => allowedTransitions;

            public GameState CreateState()
            {
                var systems = new List<IGameSystem>();
                GetSystemsRecursively(systems, transform);
                return new GameState(type, additionalScreens, systems);
            }

            void GetSystemsRecursively(List<IGameSystem> systems, Transform target)
            {
                if (TryGetComponent<IGameSystem>(out var system))
                {
                    systems.Add(system);
                }

                for (int i = 0; i < target.childCount; i++)
                {
                    var index = i;
                    GetSystemsRecursively(systems, target.GetChild(index));
                }
            }
        }
    }
}