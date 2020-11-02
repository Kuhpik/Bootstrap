using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    public sealed class GameState
    {
        public IGameSystem[] Systems { get; private set; }
        public IUpdating[] UpdateSystems { get; private set; }
        public IFixedUpdating[] FixedUpdateSystems { get; private set; }
        public bool IsInited { get; private set; }
        public EGamestate[] additionalScreens { get; private set; }

        private bool isRestarting;
        private EGamestate type;

        public GameState(EGamestate type, bool isRestarting, EGamestate[] additionalScreens, params MonoBehaviour[] systems)
        {
            Systems = systems.Select(x => x as IGameSystem).ToArray();
            this.additionalScreens = additionalScreens;
            this.isRestarting = isRestarting;
            this.type = type;
            Setup();
        }

        public void Activate(bool openScreen)
        {
            if (openScreen)
            {
                UIManager.OpenScreen(type);

                foreach (var type in additionalScreens)
                {
                    UIManager.OpenScreenAdditionaly(type);
                }
            }

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
            UpdateSystems = Systems.Where(x => x is IUpdating).Select(x => x as IUpdating).ToArray();
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