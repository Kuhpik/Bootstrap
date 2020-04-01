using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik
{
    public class GameStateInstaller : MonoBehaviour
    {
        [SerializeField] private string firstGameStateName;
        [SerializeField] private bool useGameObjects;
        [SerializeField] [ReorderableList] [HideIf("useGameObjects")] private GameStateData[] gameStateDatas;
        [SerializeField] [ReorderableList] [ShowIf("useGameObjects")] private GameStateSetuper[] gameStateSetupers;

        public FSMProcessor<GameState> InstallGameStates()
        {
            var fsm = new FSMProcessor<GameState>();

            if (useGameObjects) ProcessWithGameObjects(fsm);
            else ProcessWithSettings(fsm);

            fsm.SetState(firstGameStateName);
            return fsm;
        }

        private void ProcessWithGameObjects(FSMProcessor<GameState> fsm)
        {
            foreach (var setuper in gameStateSetupers)
            {
                var systems = new List<GameSystem>();

                for (int i = 0; i < setuper.transform.childCount; i++)
                {
                    if (setuper.transform.GetChild(i).gameObject.activeSelf)
                        systems.Add(setuper.transform.GetChild(i).GetComponent<GameSystem>());
                }

                fsm.AddState(setuper.name, new GameState(setuper.IsRestarting, systems.ToArray()), setuper.AllowedTransitions);
            }
        }

        private void ProcessWithSettings(FSMProcessor<GameState> fsm)
        {
            foreach (var data in gameStateDatas)
            {
                fsm.AddState(data.name, new GameState(data.isRestarting, data.systems), data.allowedTransitions);
            }
        }
    }
}