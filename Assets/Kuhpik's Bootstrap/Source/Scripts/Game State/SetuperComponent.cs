using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik
{
    public class SetuperComponent : MonoBehaviour
    {
        [SerializeField] GameState.Identificator type;
        [SerializeField] bool useAdditionalScreens;
        [SerializeField] bool useAdditionalStates;
        [SerializeField] [ReorderableList] GameState.Identificator[] allowedTransitions;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] GameState.Identificator[] additionalStatesInTheBegining;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] GameState.Identificator[] additionalStatesInTheEnd;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalScreens")] GameState.Identificator[] additionalScreens;

        public GameState.Identificator ID => type;
        public bool UseAdditionalScreens => useAdditionalScreens;
        public bool UseAdditionalStates => useAdditionalStates;
        public GameState.Identificator[] AdditionalStatesInTheBegining => additionalStatesInTheBegining;
        public GameState.Identificator[] AdditionalStatesInTheEnd => additionalStatesInTheEnd;
        public GameState.Identificator[] AdditionalScreens => additionalScreens;
        public GameState.Identificator[] AllowedTransitions => allowedTransitions;

        public GameState CreateState()
        {
            var systems = new List<IGameSystem>();
            GetSystemsRecursively(systems, transform);
            return new GameState(type, additionalScreens, systems);
        }

        void GetSystemsRecursively(List<IGameSystem> systems, Transform target)
        {
            if (target.TryGetComponent<IGameSystem>(out var system))
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