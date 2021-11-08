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
        public GameStateID[] OrderedStates { get; private set; } 
        public FSMProcessor<GameStateID, GameState> FSM { get; private set; }

        public override void Process()
        {
            var initialState = useArray ? gameStatesOrder[0] : firstGameState;
            var setupers = FindObjectsOfType<GameStateSetuperComponent>();
            var systemsDictionary = new Dictionary<Type, GameSystem>();
            var statesDictionary = setupers.ToDictionary(x => x.ID, x => x.CreateState());

            InitializeFSM(initialState, setupers, statesDictionary);
            HandleSharedStates(setupers);

            Bootstrap.systems = systemsDictionary;
            Bootstrap.gameStates = FSM.GetAllStates();
            Bootstrap.currentState = FSM.CurrentState;
        }

        private void InitializeFSM(GameStateID initialState, GameStateSetuperComponent[] setupers, Dictionary<GameStateID, GameState> statesDictionary)
        {
            FSM = new FSMProcessor<GameStateID, GameState>(false);

            foreach (var setuper in setupers)
            {
                var state = statesDictionary[setuper.ID];

                FSM.AddState(setuper.ID, state, setuper.AllowedTransitions);
                FSM.OnStateEnter += state.EnterState;
                FSM.OnStateExit += state.ExitState;
            }

            FSM.SetState(initialState);
        }

        private void HandleSharedStates(GameStateSetuperComponent[] setupers)
        {
            foreach (var setuper in setupers.Where(x => x.UseAdditionalStates))
            {
                var state = FSM.GetState(setuper.ID);
                var first = new List<GameState>();
                var last = new List<GameState>();

                for (int i = 0; i < setuper.AdditionalStatesInTheBegining.Length; i++)
                {
                    var additionalState = FSM.GetState(setuper.AdditionalStatesInTheBegining[i]);
                    first.Add(additionalState);
                }

                for (int i = 0; i < setuper.AdditionalStatesInTheEnd.Length; i++)
                {
                    var additionalState = FSM.GetState(setuper.AdditionalStatesInTheEnd[i]);
                    last.Add(additionalState);
                }

                state.ContactStates(first, last);
            }
        }
    }
}