﻿using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik
{
    public class GameStateSetuperComponent : MonoBehaviour
    {
        [SerializeField] GameStateID type;
        [SerializeField] bool useAdditionalStates;
        [SerializeField] [ReorderableList] GameStateID[] allowedTransitions;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] GameStateID[] additionalStatesInTheBegining;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] GameStateID[] additionalStatesInTheEnd;

        GameState state;

        public GameStateID ID => type;
        public bool UseAdditionalStates => useAdditionalStates;
        public GameStateID[] AdditionalStatesInTheBegining => additionalStatesInTheBegining;
        public GameStateID[] AdditionalStatesInTheEnd => additionalStatesInTheEnd;
        public GameStateID[] AllowedTransitions => allowedTransitions;

        public GameState CreateState()
        {
            var systems = new List<IGameSystem>();
            GetSystemsRecursively(systems, transform);
            state = new GameState(type, systems);
            return state;
        }

        public GameState GetState()
        {
            if (state == null)
            {
                CreateState();
            }

            return state;
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