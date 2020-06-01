using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik
{
    public class GameStateInstaller : MonoBehaviour
    {
        [SerializeField] private EGamestate firstGameState;
        [SerializeField] private bool useGameObjects;
        [SerializeField] [ShowIf("useGameObjects")] private bool getFromScene;
        [SerializeField] [ReorderableList] [HideIf("useGameObjects")] private GameStateData[] gameStateDatas;
        [SerializeField] [ReorderableList] [HideIf("getFromScene")] private GameStateSetuper[] gameStateSetupers;

        public FSMProcessor<GameState> InstallGameStates()
        {
            var fsm = new FSMProcessor<GameState>();

            if (useGameObjects) ProcessWithGameObjects(fsm);
            else ProcessWithSettings(fsm);

            fsm.SetState(firstGameState.GetName());
            return fsm;
        }

        private void ProcessWithGameObjects(FSMProcessor<GameState> fsm)
        {
            var setupers = getFromScene ? FindObjectsOfType<GameStateSetuper>() : gameStateSetupers;

            foreach (var setuper in setupers)
            {
                var systems = new List<GameSystem>();

                for (int i = 0; i < setuper.transform.childCount; i++)
                {
                    if (setuper.transform.GetChild(i).gameObject.activeSelf)
                        systems.Add(setuper.transform.GetChild(i).GetComponent<GameSystem>());
                }

                fsm.AddState(setuper.Type.GetName(), new GameState(setuper.Type, setuper.IsRestarting, systems.ToArray()), setuper.AllowedTransitions.GetNames());
            }
        }

        private void ProcessWithSettings(FSMProcessor<GameState> fsm)
        {
            foreach (var data in gameStateDatas)
            {
                fsm.AddState(data.type.GetName(), new GameState(data.type, data.isRestarting, data.systems), data.allowedTransitions.GetNames());
            }
        }
    }
}