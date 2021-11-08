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
        static GameStateID lastState;

        internal static GameState currentState;
        internal static GameState[] gameStates;
        internal static Dictionary<Type, GameSystem> systems;
        internal static List<Object> itemsToInject = new List<Object>();

        internal static event Action SaveEvent;
        internal static event Action GameStartEvent;
        internal static event Action GamePreStartEvent;
        internal static event Action GameEndEvent;
        internal static event Action<GameStateID> OnStateChangedEvent;

        void Awake()
        {
            SaveEvent = null;
            GameStartEvent = null;
            GamePreStartEvent = null;
            OnStateChangedEvent = null;
            GameEndEvent = null;
        }

        void Start()
        {
            itemsToInject.Add(config);
            itemsToInject.Add(new GameData());

            var installers = FindObjectsOfType<Installer>().OrderBy(x => x.Order).ToArray();

            for (int i = 0; i < installers.Length; i++)
            {
                installers[i].Process();
            }

            GamePreStartEvent?.Invoke();
            GameStartEvent?.Invoke();
        }

        void Update()
        {
            currentState.Update();
        }

        void FixedUpdate()
        {
            currentState.FixedUpdate();
        }

        public static void GameRestart(int sceneIndex)
        {
            GameEndEvent?.Invoke();
            SaveGame();

            SceneManager.LoadScene(sceneIndex);
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
            lastState = GetCurrentGamestate();
            OnStateChangedEvent?.Invoke(id);
        }

        public static T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        public static GameStateID GetCurrentGamestate()
        {
            return currentState.ID;
        }

        public static GameStateID GetLastGamestate()
        {
            return lastState;
        }
    }
}