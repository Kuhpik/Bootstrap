using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Kuhpik
{
    public sealed class FSMProcessor<TKey, TState>
    {
        public TState State { get; private set; }

        TKey currentStateKey;
        Dictionary<TKey, TState> states = new Dictionary<TKey, TState>();
        Dictionary<TKey, IEnumerable<TKey>> allowedTransition = new Dictionary<TKey, IEnumerable<TKey>>();

        public FSMProcessor() { }

        public FSMProcessor(TKey key, TState state, params string[] allowedTransitions) : base()
        {
            currentStateKey = key;
            State = state;
            AddState(key, state);
        }

        public void AddState(TKey key, TState state, params TKey[] allowedTransitions)
        {
            states.Add(key, state);
            AddTransition(key, allowedTransitions);
        }

        public TState[] GetAllStates()
        {
            return states.Values.ToArray();
        }

        public TState GetState(TKey key)
        {
            return states[key];
        }

        public void ChangeState(TKey key)
        {
            if (allowedTransition.ContainsKey(currentStateKey) && !allowedTransition[currentStateKey].Contains(key))
            {
                Debug.LogError($"Not allowed transition from {currentStateKey} to {key}!");
            }
            else
            {
                Debug.Log($"State changed to {key}!");
                State = states[key];
                currentStateKey = key;
            }
        }

        public async void ChangeState(TKey key, float delay)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));
            ChangeState(key);
        }

        public void SetState(TKey key)
        {
            if (State != null) return;

            currentStateKey = key;
            State = states[key];
        }

        public void AddTransition(TKey key, params TKey[] allowedTransitions)
        {
            allowedTransition.Add(key, allowedTransitions);
        }
    }
}