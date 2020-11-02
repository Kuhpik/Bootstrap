using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    public class GameStateInstaller : MonoBehaviour
    {
        [SerializeField] private bool useArray;
        [SerializeField] [ShowIf("useArray")] private EGamestate[] gameStatesOrder;
        [SerializeField] [HideIf("useArray")] private EGamestate firstGameState;
        [SerializeField] [ShowIf("useGameObjects")] private bool getFromScene;
        [SerializeField] [ReorderableList] [HideIf("getFromScene")] private GameStateSetuper[] gameStateSetupers;

        public void InstallGameStates(out FSMProcessor<GameState> fsm, out string[] order)
        {
            order = useArray ? gameStatesOrder.Select(x => x.GetName()).ToArray() : new string[] { firstGameState.GetName() };
            fsm = new FSMProcessor<GameState>();
            ProcessWithGameObjects(fsm);
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

                fsm.AddState(setuper.Type.GetName(), new GameState(setuper.Type, setuper.IsRestarting, setuper.OpenAdditionalScreens? setuper.AdditionalScreens : new EGamestate[0], systems.ToArray()), setuper.AllowedTransitions.GetNames());
            }
        }
    }
}