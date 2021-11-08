using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    public class GameStateInstaller : Installer
    {
        [SerializeField] bool useArray;
        [SerializeField] [HideIf("useArray")] GameStateID firstGameState;
        [SerializeField] [ShowIf("useArray")] GameStateID[] gameStatesOrder;

        public override int Order => 1;

        FSMProcessor<GameStateID, GameState> fsm;

        public override void Process()
        {
            var launchStates = useArray ? gameStatesOrder : new GameStateID[] { firstGameState };
            var setupers = FindObjectsOfType<GameStateSetuperComponent>();
            var systemsDictionary = new Dictionary<Type, GameSystem>();
            var statesDictionary = setupers.ToDictionary(x => x.ID, x => x.CreateState());

            InitializeFSM(setupers, statesDictionary);
            HandleSharedStates(setupers);

            Bootstrap.systems = systemsDictionary;
            Bootstrap.currentState = fsm.CurrentState;
            Bootstrap.launchStates = launchStates;
            Bootstrap.FSM = fsm;
        }

        private void InitializeFSM(GameStateSetuperComponent[] setupers, Dictionary<GameStateID, GameState> statesDictionary)
        {
            fsm = new FSMProcessor<GameStateID, GameState>(false);

            foreach (var setuper in setupers)
            {
                var state = statesDictionary[setuper.ID];

                fsm.AddState(setuper.ID, state, setuper.AllowedTransitions);
                SubscribeStateToEvents(state);
            }
        }

        private void SubscribeStateToEvents(GameState state)
        {
            fsm.OnStateEnter += state.EnterState;
            fsm.OnStateExit += state.ExitState;

            Bootstrap.GameStartEvent += state.GameStart;
            Bootstrap.GameEndEvent += state.GameEnd;
        }

        private void HandleSharedStates(GameStateSetuperComponent[] setupers)
        {
            foreach (var setuper in setupers.Where(x => x.UseAdditionalStates))
            {
                var state = fsm.GetState(setuper.ID);
                var first = new List<GameState>();
                var last = new List<GameState>();

                for (int i = 0; i < setuper.AdditionalStatesInTheBegining.Length; i++)
                {
                    var additionalState = fsm.GetState(setuper.AdditionalStatesInTheBegining[i]);
                    first.Add(additionalState);
                }

                for (int i = 0; i < setuper.AdditionalStatesInTheEnd.Length; i++)
                {
                    var additionalState = fsm.GetState(setuper.AdditionalStatesInTheEnd[i]);
                    last.Add(additionalState);
                }

                state.ContactStates(first, last);
            }
        }
    }
}