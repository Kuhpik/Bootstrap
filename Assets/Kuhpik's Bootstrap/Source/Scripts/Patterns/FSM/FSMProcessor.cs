using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    public sealed class FSMProcessor<TKey, TState>
    {
        public TState CurrentState { get; private set; }
        public TKey CurrentStateKey { get; private set; }
        public readonly bool IsAnyTransitionAllowed;

        public event Action<TKey> OnStateEnter;
        public event Action<TKey> OnStateExit;

        readonly Dictionary<TKey, TState> states;
        readonly Dictionary<TKey, IEnumerable<TKey>> allowedTransition;

        public FSMProcessor(bool allowAnyTransition)
        {
            states = new Dictionary<TKey, TState>();
            IsAnyTransitionAllowed = allowAnyTransition;
            allowedTransition = new Dictionary<TKey, IEnumerable<TKey>>();
        }

        public void SetState(TKey key)
        {
            if (CurrentState != null) OnStateExit?.Invoke(CurrentStateKey);

            Debug.Log($"State changed to <color=orange>{key}</color>!");
            CurrentState = states[key];
            CurrentStateKey = key;

            OnStateEnter?.Invoke(key);
        }

        public void ChangeState(TKey key)
        {
            if (IsAnyTransitionAllowed) SwitchStateIgnoringTransitions(key);
            else SwitchState(key);
        }

        public void AddState(TKey key, TState state, IEnumerable<TKey> allowedTransitions)
        {
            if (!states.ContainsKey(key))
            {
                states.Add(key, state);
                allowedTransition.Add(key, allowedTransitions);
            }

            else
            {
                Debug.LogError($"State with key {key} already exist in collection");
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

        void SwitchState(TKey key)
        {
            if (allowedTransition.ContainsKey(CurrentStateKey) && !allowedTransition[CurrentStateKey].Contains(key))
            {
                Debug.LogError($"Not allowed transition from {CurrentStateKey} to {key}!");
            }

            else
            {
                SetState(key);
            }
        }

        void SwitchStateIgnoringTransitions(TKey key)
        {
            SetState(key);
        }
    }
}