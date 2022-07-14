using NaughtyAttributes;
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
            var setupers = FindObjectsOfType<GameStateComponent>();
            var statesDictionary = setupers.ToDictionary(x => x.ID, x => x.CreateState());

            InitializeFSM(setupers, statesDictionary);
            HandleSharedStates(setupers);

            Bootstrap.Instance.launchStates = launchStates;
            Bootstrap.Instance.fsm = fsm;
        }

        private void InitializeFSM(GameStateComponent[] setupers, Dictionary<GameStateID, GameState> statesDictionary)
        {
            fsm = new FSMProcessor<GameStateID, GameState>();

            foreach (var setuper in setupers)
            {
                var state = statesDictionary[setuper.ID];

                fsm.AddState(setuper.ID, state);
                SubscribeStateToEvents(state);
            }
        }

        private void SubscribeStateToEvents(GameState state)
        {
            fsm.OnStateEnter += state.EnterState;
            fsm.OnStateExit += state.ExitState;

            Bootstrap.Instance.GameStartEvent += state.GameStart;
            Bootstrap.Instance.GameEndEvent += state.GameEnd;
        }

        private void HandleSharedStates(GameStateComponent[] setupers)
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

                state.JoinStates(first, last);
            }
        }
    }
}