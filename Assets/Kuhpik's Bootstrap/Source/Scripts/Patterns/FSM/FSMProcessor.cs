using Kuhpik.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    public sealed class FSMProcessor<TKey, TState>
    {
        public TKey CurrentStateKey { get; private set; }
        public TState CurrentState { get; private set; }

        public event Action<TKey> OnStateEnter;
        public event Action<TKey> OnStateExit;

        readonly Dictionary<TKey, TState> states;

        public FSMProcessor()
        {
            states = new Dictionary<TKey, TState>();
        }

        public void ChangeState(TKey key)
        {
            if (CurrentState != null) OnStateExit?.Invoke(CurrentStateKey);

            LogExtensions.Log($"State changed to <color=orange>{key}</color>");

            CurrentState = states[key];
            CurrentStateKey = key;

            OnStateEnter?.Invoke(key);
        }

        public void AddState(TKey key, TState state)
        {
            if (!states.ContainsKey(key))
            {
                states.Add(key, state);
            }
        }

        public TState[] GetAllStates()
        {
            return states.Values.ToArray();
        }

        public TState GetState(TKey key)
        {
            return states[key];
        }
    }
}