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
        [SerializeField] [ShowIf("useArray")] EGamestate[] gameStatesOrder;
        [SerializeField] [HideIf("useArray")] EGamestate firstGameState;
        [SerializeField] [ReorderableList] [HideIf("getFromScene")] GameStateSetuper[] gameStateSetupers;

        FSMProcessor<GameState> fsm;
        EGamestate[] statesOrder;

        void Start()
        {
            InstallGameStates(out fsm, out statesOrder);
            Bootstrap.OnGameStartEvent += ActivateStates;
            Bootstrap.OnStateChangedEvent += ChangeGameState;
        }

        void InstallGameStates(out FSMProcessor<GameState> fsm, out EGamestate[] order)
        {
            order = useArray ? gameStatesOrder.Select(x => x).ToArray() : new EGamestate[] { firstGameState };
            fsm = new FSMProcessor<GameState>();
            ProcessWithGameObjects(fsm);
        }

        void ProcessWithGameObjects(FSMProcessor<GameState> fsm)
        {
            var setupers = getFromScene ? FindObjectsOfType<GameStateSetuper>() : gameStateSetupers;
            var systemsDictionary = new Dictionary<Type, GameSystem>();

            foreach (var setuper in setupers)
            {
                var systems = new List<GameSystem>();

                for (int i = 0; i < setuper.transform.childCount; i++)
                {
                    if (setuper.transform.GetChild(i).gameObject.activeSelf)
                    {
                        var system = setuper.transform.GetChild(i).GetComponent<GameSystem>();
                        systemsDictionary.Add(system.GetType(), system);
                        systems.Add(system);
                    }
                }

                fsm.AddState(setuper.Type.GetName(), new GameState(setuper.Type, setuper.IsRestarting, setuper.OpenAdditionalScreens ? setuper.AdditionalScreens : new EGamestate[0], systems.ToArray()), setuper.AllowedTransitions.GetNames());
            }

            fsm.SetState(useArray ? gameStatesOrder[0].GetName() : firstGameState.GetName());

            Bootstrap.gameStates = fsm.GetAllStates<GameState>();
            Bootstrap.systems = systemsDictionary;
            Bootstrap.currentState = fsm.State;
        }

        void ActivateStates()
        {
            fsm.State.Activate(true);

            for (int i = 1; i < statesOrder.Length; i++)
            {
                ChangeGameState(statesOrder[i]);
            }
        }

        void ChangeGameState(EGamestate type)
        {
            fsm.State.Deactivate();
            fsm.ChangeState(type.GetName());
            fsm.State.Activate(true);

            Bootstrap.currentState = fsm.State;
        }
    }
}