using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Kuhpik
{
    public sealed class FSMProcessor<T>
    {
        public T State { get; private set; }

        private Dictionary<string, T> states = new Dictionary<string, T>();
        private Dictionary<string, IEnumerable<string>> allowedTransition = new Dictionary<string, IEnumerable<string>>();

        private string currentStateName;

        public FSMProcessor()
        {
        }

        public FSMProcessor(string name, T state, params string[] allowedTransitions)
        {
            currentStateName = name;
            State = state;
            AddState(name, state);
        }

        public void AddState(string name, T state, params string[] allowedTransitions)
        {
            states.Add(name, state);
            AddTransition(name, allowedTransitions);
        }

        public void ChangeState(string name)
        {
            if (allowedTransition.ContainsKey(currentStateName) && !allowedTransition[currentStateName].Contains(name))
            {
                Debug.LogError($"Not allowed transition from {currentStateName} to {name}!");
            }
            else
            {
                Debug.Log($"State changed to {name}!");
                State = states[name];
                currentStateName = name;
            }
        }

        public async void ChangeState(string name, float delay)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));
            ChangeState(name);
        }

        public void SetState(string name)
        {
            if (State != null) return;

            currentStateName = name;
            State = states[name];
        }

        public void AddTransition(string name, params string[] allowedTransitions)
        {
            allowedTransition.Add(name, allowedTransitions);
        }
    }
}