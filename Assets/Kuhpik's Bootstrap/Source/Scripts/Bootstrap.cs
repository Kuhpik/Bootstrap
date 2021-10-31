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
        static GameState.Identificator lastState;

        internal static GameState currentState;
        internal static GameState[] gameStates;
        internal static Dictionary<Type, GameSystem> systems;
        internal static List<Object> itemsToInject = new List<Object>();

        internal static event Action OnSaveEvent;
        internal static event Action OnGameStartEvent;
        internal static event Action OnGamePreStartEvent;
        internal static event Action<GameState.Identificator> OnStateChangedEvent;

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

            var installers = FindObjectsOfType<Installer>().OrderBy(x => x.Order).ToArray();

            for (int i = 0; i < installers.Length; i++)
            {
                installers[i].Process();
            }

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

        public static void ChangeGameState(GameState.Identificator id)
        {
            lastState = GetCurrentGamestate();
            OnStateChangedEvent?.Invoke(id);
        }

        public static T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        public static GameState.Identificator GetCurrentGamestate()
        {
            return currentState.ID;
        }

        public static GameState.Identificator GetLastGamestate()
        {
            return lastState;
        }
    }
}