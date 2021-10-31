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
        public readonly bool IsAnyTransitionsAllowed;

        public event Action<TKey> OnStateEnter;
        public event Action<TKey> OnStateExit;

        readonly Dictionary<TKey, TState> states;
        readonly Dictionary<TKey, IEnumerable<TKey>> allowedTransition;

        /// <summary>
        /// Creates FSM with all posible states and transitions between them.
        /// </summary>
        /// <param name="datas">Custom tuple for important data</param>
        /// <param name="initialState">Initial state of this FSM</param>
        /// <param name="allowAnyTransition">Should FSM ignore transition data?</param>
        public FSMProcessor(IEnumerable<(TKey, TState, IEnumerable<TKey>)> datas, TKey initialState, bool allowAnyTransition) : base()
        {
            allowedTransition = datas.ToDictionary(x => x.Item1, x => x.Item3);
            states = datas.ToDictionary(x => x.Item1, x => x.Item2);
            IsAnyTransitionsAllowed = allowAnyTransition;
            SetState(initialState);
        }

        public void ChangeState(TKey key)
        {
            if (IsAnyTransitionsAllowed) SwitchStateIgnoringTransitions(key);
            else SwitchState(key);
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

        void SetState(TKey key)
        {
            Debug.Log($"State changed to <color=orange>{key}</color>!");
            CurrentState = states[key];
            CurrentStateKey = key;
        }
    }
}