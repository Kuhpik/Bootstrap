using System.Threading.Tasks;
using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Kuhpik
{
    public sealed class GameState
    {
        public IGameSystem[] Systems { get; private set; }

        //Avoid casting. Have to wait till C# 8.0
        public IIniting[] InitingSystem { get; private set; }
        public IUpdating[] UpdateSystems { get; private set; }
        public IDisposing[] DisposingSystem { get; private set; }
        public IFixedUpdating[] FixedUpdateSystems { get; private set; }

        public EGamestate[] AdditionalScreens { get; private set; }
        public EGamestate Type { get; private set; }

        IFixedUpdating[] setupedFixedUpdateSystem;
        IUpdating[] setupedUpdateSystems;

        public bool isInited;
        bool isRestarting;

        public GameState(EGamestate type, bool isRestarting, EGamestate[] additionalScreens, bool setupImmediate, params MonoBehaviour[] systems)
        {
            Systems = systems.Select(x => x as IGameSystem).ToArray();
            AdditionalScreens = additionalScreens;
            this.isRestarting = isRestarting;
            Type = type;

            if (setupImmediate) Setup();
        }

        public void ContactSystems(IEnumerable<IGameSystem> systemsInTheBegining, IEnumerable<IGameSystem> systemsInTheEnd)
        {
            var contacted = new List<IGameSystem>();

            contacted.AddRange(systemsInTheBegining);
            contacted.AddRange(Systems);
            contacted.AddRange(systemsInTheEnd);

            Systems = contacted.ToArray();

            Setup();
        }

        public void Activate(bool openScreen)
        {
            HandleInit();
            HandleScreens(openScreen);
            PrepareUpdatingSystems();
        }

        public void Deactivate(bool ignoreCheck = false)
        {
            if ((isRestarting && isInited) || ignoreCheck)
            {
                for (int i = 0; i < DisposingSystem.Length; i++)
                {
                    DisposingSystem[i].OnDispose();
                }

                isInited = false;
            }
        }

        void HandleInit()
        {
            if (isRestarting || !isInited)
            {
                for (int i = 0; i < InitingSystem.Length; i++)
                {
                    InitingSystem[i].OnInit();
                }

                isInited = true;
            }
        }

        void HandleScreens(bool openScreen)
        {
            if (openScreen)
            {
                UIManager.OpenScreen(Type);

                foreach (var type in AdditionalScreens)
                {
                    UIManager.OpenScreenAdditionaly(type);
                }
            }
        }

        async void PrepareUpdatingSystems()
        {
            UpdateSystems = Array.Empty<IUpdating>();
            FixedUpdateSystems = Array.Empty<IFixedUpdating>();

            await Task.Yield();

            FixedUpdateSystems = setupedFixedUpdateSystem;
            UpdateSystems = setupedUpdateSystems;
        }

        void Setup()
        {
            InitingSystem = Systems.Where(x => x is IIniting).Select(x => x as IIniting).ToArray();
            DisposingSystem = Systems.Where(x => x is IDisposing).Select(x => x as IDisposing).ToArray();
            setupedUpdateSystems = Systems.Where(x => x is IUpdating).Select(x => x as IUpdating).ToArray();
            setupedFixedUpdateSystem = Systems.Where(x => x is IFixedUpdating).Select(x => x as IFixedUpdating).ToArray();
        }
    }
}