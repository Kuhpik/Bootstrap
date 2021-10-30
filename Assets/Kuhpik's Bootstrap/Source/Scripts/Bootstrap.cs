using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Kuhpik
{
    [DefaultExecutionOrder(500)]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] GameConfig config;
        static GameStateName lastState;

        internal static GameState currentState;
        internal static GameState[] gameStates;
        internal static Dictionary<Type, GameSystem> systems;
        internal static List<Object> itemsToInject = new List<Object>();

        internal static event Action OnSaveEvent;
        internal static event Action OnGameStartEvent;
        internal static event Action OnGamePreStartEvent;
        internal static event Action<GameStateName> OnStateChangedEvent;

        void Awake()
        {
            OnSaveEvent = null;
            OnGameStartEvent = null;
            OnGamePreStartEvent = null;
            OnStateChangedEvent = null;
        }

        void Start()
        {
            itemsToInject.Add(config);
            itemsToInject.Add(new GameData());

            OnGamePreStartEvent?.Invoke();
            OnGameStartEvent?.Invoke();
        }

        void Update()
        {
            currentState.RunUpdate();
        }

        void FixedUpdate()
        {
            currentState.RunFixedUpdate();
        }

        public static void GameRestart(int sceneIndex)
        {
            foreach (var state in gameStates)
            {
                state.Deactivate(true);
            }

            SaveGame();
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Saves all changes in Player Data to PlayerPrefs.
        /// On level complete\fail use GameRestart() instead.
        /// </summary>
        public static void SaveGame()
        {
            OnSaveEvent?.Invoke();
        }

        public static void ChangeGameState(GameStateName type)
        {
            lastState = GetCurrentGamestate();
            OnStateChangedEvent?.Invoke(type);
        }

        public static T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        public static GameStateName GetCurrentGamestate()
        {
            return currentState.ID;
        }

        public static GameStateName GetLastGamestate()
        {
            return lastState;
        }
    }
}