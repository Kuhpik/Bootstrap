﻿using System.Collections.Generic;

namespace Kuhpik
{
    public sealed partial class GameState
    {
        readonly public IGameSystem[] Systems;
        readonly public GameState[] StatesIncludingSubstates;
        readonly public Identificator[] AdditionalScreens;
        readonly public Identificator ID;

        bool isInited;

        public GameState(Identificator id, Identificator[] additionalScreens, IEnumerable<IGameSystem> systems)
        {
            //Systems = systems.Select(x => x as IGameSystem).ToArray();
            //StatesIncludingSubstates = new GameState[] { this };
            //AdditionalScreens = additionalScreens;
            //this.isRestarting = isRestarting;
            //Type = type;
            //Setup();
        }

        public void ContactStates(IEnumerable<GameState> statesInTheBegining, IEnumerable<GameState> statesInTheEnd)
        {
            //var contacted = new List<GameState>();

            //contacted.AddRange(statesInTheBegining);
            //contacted.AddRange(StatesIncludingSubstates);
            //contacted.AddRange(statesInTheEnd);

            //StatesIncludingSubstates = contacted.ToArray();
        }

        public void Activate()
        {
            //for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            //{
            //    var state = StatesIncludingSubstates[i];

            //    HandleScreens(state);
            //    PrepareUpdatingSystems(state);
            //    HandleInit(state);
            //}
        }

        public void Deactivate(bool ignoreCheck = false)
        {
            //if (!ignoreCheck)
            //{
            //    for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            //    {
            //        var state = StatesIncludingSubstates[i];

            //        if ((state.isRestarting && state.isInited) || ignoreCheck)
            //        {
            //            for (int j = 0; j < state.DisposingSystem.Length; j++)
            //            {
            //                state.DisposingSystem[j].OnDispose();
            //            }

            //            state.isInited = false;
            //        }
            //    }
            //}

            //else
            //{
            //    for (int i = 0; i < DisposingSystem.Length; i++)
            //    {
            //        DisposingSystem[i].OnDispose();
            //    }
            //}
        }

        public void RunUpdate()
        {
            //for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            //{
            //    for (int j = 0; j < StatesIncludingSubstates[i].UpdateSystems.Length; j++)
            //    {
            //        StatesIncludingSubstates[i].UpdateSystems[j].OnUpdate();
            //    }
            //}
        }

        public void RunFixedUpdate()
        {
            //for (int i = 0; i < StatesIncludingSubstates.Length; i++)
            //{
            //    for (int j = 0; j < StatesIncludingSubstates[i].FixedUpdateSystems.Length; j++)
            //    {
            //        StatesIncludingSubstates[i].FixedUpdateSystems[j].OnFixedUpdate();
            //    }
            //}
        }

        void HandleInit(GameState state)
        {
            //if (state.isRestarting || !state.isInited)
            //{
            //    for (int i = 0; i < state.InitingSystem.Length; i++)
            //    {
            //        state.InitingSystem[i].OnInit();
            //    }

            //    state.isInited = true;
            //}
        }

        void HandleScreens(GameState state)
        {
            // Only open current state's screen.
            // Otherwise additional screens in the end will close main state's UI screen;
            //if (state == this) UIManager.OpenScreen(state.Type);

            //foreach (var type in state.AdditionalScreens)
            //{
            //    UIManager.OpenScreenAdditionaly(type);
            //}
        }

        async void PrepareUpdatingSystems(GameState state)
        {
            //state.UpdateSystems = Array.Empty<IUpdating>();
            //state.FixedUpdateSystems = Array.Empty<IFixedUpdating>();

            //await Task.Yield();

            //state.FixedUpdateSystems = state.setupedFixedUpdateSystem;
            //state.UpdateSystems = state.setupedUpdateSystems;
        }

        void Setup()
        {
            //InitingSystem = Systems.Where(x => x is IIniting).Select(x => x as IIniting).ToArray();
            //DisposingSystem = Systems.Where(x => x is IDisposing).Select(x => x as IDisposing).ToArray();
            //setupedUpdateSystems = Systems.Where(x => x is IUpdating).Select(x => x as IUpdating).ToArray();
            //setupedFixedUpdateSystem = Systems.Where(x => x is IFixedUpdating).Select(x => x as IFixedUpdating).ToArray();
        }
    }
}