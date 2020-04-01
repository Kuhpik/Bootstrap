using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    [System.Serializable]
    public struct GameStateData
    {
        public string name;
        public GameSystem[] systems;
        public string allowedTransitions;
        public bool isRestarting;
    }

    public sealed class GameState
    {
        public IGameSystem[] Systems { get; private set; }
        public IRunning[] RunningSystems { get; private set; }
        public bool IsInited { get; private set; }

        private bool isRestarting;

        public GameState(bool isRestarting, params MonoBehaviour[] systems)
        {
            Systems = systems.Select(x => x as IGameSystem).ToArray();
            this.isRestarting = isRestarting;
            Setup();
        }

        public void Activate()
        {
            if (isRestarting || !IsInited)
            {
                Perform<IIniting>();
                IsInited = true;
            }
        }

        public void Deactivate()
        {
            if (isRestarting && IsInited)
            {
                Perform<IDisposing>();
                IsInited = false;
            }
        }

        private void Setup()
        {
            var runnings = new List<IRunning>();

            for (int i = 0; i < Systems.Length; i++)
            {
                if (Systems[i] is IRunning) runnings.Add(Systems[i] as IRunning);
            }

            RunningSystems = runnings.ToArray();
        }

        private void Perform<T>() where T : IGameSystem
        {
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i].PerformAction<T>();
            }
        }
    }
}