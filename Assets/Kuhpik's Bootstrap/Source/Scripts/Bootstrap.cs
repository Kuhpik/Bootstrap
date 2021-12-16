using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Kuhpik
{
    [DefaultExecutionOrder(500)]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] GameConfig config;

        internal static GameStateID[] launchStates;
        internal static Dictionary<Type, GameSystem> systems;
        internal static List<Object> itemsToInject = new List<Object>();

        internal static event Action GamePreStartEvent;
        internal static event Action GameStartEvent;
        internal static event Action GameEndEvent;
        internal static event Action SaveEvent;

        internal static event Action<GameStateID> StateEnterEvent;
        internal static event Action<GameStateID> StateExitEvent;

        internal static FSMProcessor<GameStateID, GameState> FSM;

        static GameState lastState;
        static GameState currentState;

        void Start()
        {
            itemsToInject.Add(config);
            itemsToInject.Add(new GameData());

            systems = FindObjectsOfType<GameSystem>().ToDictionary(x => x.GetType(), x => x);

            ProcessInstallers();

            GamePreStartEvent?.Invoke();
            GameStartEvent?.Invoke();

            LaunchStates();
        }

        void Update()
        {
            currentState.Update();
        }

        void LateUpdate()
        {
            currentState.LateUpdate();
        }

        void FixedUpdate()
        {
            currentState.FixedUpdate();
        }

        public static void GameRestart(int sceneIndex)
        {
            GameEndEvent?.Invoke();

            ResetEvent();
            SaveGame();

            SceneManager.LoadScene(sceneIndex);
        }

        private static void ResetEvent()
        {
            SaveEvent = null;
            GameStartEvent = null;
            GamePreStartEvent = null;
            GameEndEvent = null;
            StateEnterEvent = null;
            StateExitEvent = null;
        }

        /// <summary>
        /// Saves all changes in Player Data to PlayerPrefs.
        /// On level complete\fail use GameRestart() instead.
        /// </summary>
        public static void SaveGame()
        {
            SaveEvent?.Invoke();
        }

        public static void ChangeGameState(GameStateID id)
        {
            if (lastState != null)
            {
                StateExitEvent?.Invoke(lastState.ID);
                lastState = currentState;
            }

            FSM.ChangeState(id);
            StateEnterEvent?.Invoke(id);
            currentState = FSM.CurrentState;
        }

        public static T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        public static GameStateID GetCurrentGamestateID()
        {
            return currentState.ID;
        }

        public static GameStateID GetLastGamestateID()
        {
            return lastState.ID;
        }

        void LaunchStates()
        {
            for (int i = 0; i < launchStates.Length; i++)
            {
                ChangeGameState(launchStates[i]);
            }
        }

        void ProcessInstallers()
        {
            var installers = FindObjectsOfType<Installer>().OrderBy(x => x.Order).ToArray();

            for (int i = 0; i < installers.Length; i++)
            {
                installers[i].Process();
            }
        }
    }
}