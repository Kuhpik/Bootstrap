using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    public class GameStateInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] bool useArray;
        [SerializeField] [HideIf("useArray")] GameState.Identificator firstGameState;
        [SerializeField] [ShowIf("useArray")] GameState.Identificator[] gameStatesOrder;

        public int Order => 1;
        public GameState.Identificator[] OrderedStates { get; private set; }
        public FSMProcessor<GameState.Identificator, GameState> FSM { get; private set; }

        public void Process()
        {
            var initialState = useArray ? gameStatesOrder[0] : firstGameState;
            var setupers = FindObjectsOfType<GameState.SetuperComponent>();
            var systemsDictionary = new Dictionary<Type, GameSystem>();
            var statesDictionary = setupers.ToDictionary(x => x.ID, x => x.CreateState());

            InitializeFSM(initialState, setupers, statesDictionary);
            HandleSharedStates(setupers);

            Bootstrap.systems = systemsDictionary;
            Bootstrap.gameStates = FSM.GetAllStates();
            Bootstrap.currentState = FSM.CurrentState;
        }

        private void InitializeFSM(GameState.Identificator initialState, GameState.SetuperComponent[] setupers, Dictionary<GameState.Identificator, GameState> statesDictionary)
        {
            FSM = new FSMProcessor<GameState.Identificator, GameState>(false);

            foreach (var setuper in setupers)
            {
                FSM.AddState(setuper.ID, statesDictionary[setuper.ID], setuper.AllowedTransitions);
            }

            FSM.SetState(initialState);
        }

        private void HandleSharedStates(GameState.SetuperComponent[] setupers)
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