using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    [System.Serializable]
    public struct GameStateData
    {
        public EGamestate type;
        public GameSystem[] systems;
        public EGamestate[] allowedTransitions;
        public bool isRestarting;
    }

    public sealed class GameState
    {
        public IGameSystem[] Systems { get; private set; }
        public IRunning[] RunningSystems { get; private set; }
        public IFixedUpdating[] FixedUpdateSystems { get; private set; }
        public bool IsInited { get; private set; }

        private bool isRestarting;
        private EGamestate type;

        public GameState(EGamestate type, bool isRestarting, params MonoBehaviour[] systems)
        {
            Systems = systems.Select(x => x as IGameSystem).ToArray();
            this.isRestarting = isRestarting;
            this.type = type;
            Setup();
        }

        public void Activate()
        {
            UIManager.OpenScreen(type);

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
            RunningSystems = Systems.Where(x => x is IRunning).Select(x => x as IRunning).ToArray();
            FixedUpdateSystems = Systems.Where(x => x is IFixedUpdating).Select(x => x as IFixedUpdating).ToArray();
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