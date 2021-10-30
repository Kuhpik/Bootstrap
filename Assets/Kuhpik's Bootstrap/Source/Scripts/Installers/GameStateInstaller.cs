using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    [DefaultExecutionOrder(10)]
    public class GameStateInstaller : MonoBehaviour
    {
        [SerializeField] bool useArray;
        [SerializeField] bool getFromScene;
        [SerializeField] [ShowIf("useArray")] GameStateName[] gameStatesOrder;
        [SerializeField] [HideIf("useArray")] GameStateName firstGameState;
        [SerializeField] [ReorderableList] [HideIf("getFromScene")] GameStateSetuper[] gameStateSetupers;

        FSMProcessor<GameStateName, GameState> fsm;
        GameStateName[] statesOrder;

        void Start()
        {
            InstallGameStates(out fsm, out statesOrder);
            Bootstrap.OnGameStartEvent += ActivateStates;
            Bootstrap.OnStateChangedEvent += ChangeGameState;
        }

        void InstallGameStates(out FSMProcessor<GameStateName, GameState> fsm, out GameStateName[] order)
        {
            order = useArray ? gameStatesOrder.Select(x => x).ToArray() : new GameStateName[] { firstGameState };
            fsm = new FSMProcessor<GameStateName, GameState>();
            ProcessWithGameObjects(fsm);
        }

        void ProcessWithGameObjects(FSMProcessor<GameStateName , GameState> fsm)
        {
            var setupers = getFromScene ? FindObjectsOfType<GameStateSetuper>() : gameStateSetupers;
            var systemsDictionary = new Dictionary<Type, GameSystem>();

            //Prepare all Gamestates
            foreach (var setuper in setupers)
            {
                var systems = new List<GameSystem>();

                for (int i = 0; i < setuper.transform.childCount; i++)
                {
                    if (setuper.transform.GetChild(i).gameObject.activeSelf)
                    {
                        if (setuper.transform.GetChild(i).TryGetComponent<GameSystem>(out var system))
                        {
                            systemsDictionary.Add(system.GetType(), system);
                            systems.Add(system);
                        }
                    }
                }

                fsm.AddState(setuper.Type, new GameState(setuper.Type, setuper.IsRestarting, setuper.UseAdditionalScreens ? setuper.AdditionalScreens : new GameStateName[0], systems.ToArray()), setuper.AllowedTransitions);
            }

            //Handle one's that has additional states
            foreach (var setuper in setupers.Where(x => x.UseAdditionalStates))
            {
                var state = fsm.GetState(setuper.Type);
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

            fsm.SetState(useArray ? gameStatesOrder[0] : firstGameState);

            Bootstrap.gameStates = fsm.GetAllStates();
            Bootstrap.systems = systemsDictionary;
            Bootstrap.currentState = fsm.State;
        }

        void ActivateStates()
        {
            fsm.State.Activate();

            for (int i = 1; i < statesOrder.Length; i++)
            {
                ChangeGameState(statesOrder[i]);
            }
        }

        void ChangeGameState(GameStateName type)
        {
            fsm.State.Deactivate();
            fsm.ChangeState(type);
            fsm.State.Activate();

            Bootstrap.currentState = fsm.State;
        }
    }
}