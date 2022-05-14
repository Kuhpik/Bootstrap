using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Kuhpik
{
    [DefaultExecutionOrder(500)]
    public class Bootstrap : Singleton<Bootstrap>
    {
        [SerializeField] GameConfig config;

        public PlayerData PlayerData { get; private set; }
        public GameData GameData { get; private set; }

        internal GameStateID[] launchStates;
        internal Dictionary<Type, GameSystem> systems;
        internal List<Object> itemsToInject = new List<Object>();

        internal event Action GamePreStartEvent;
        internal event Action GameStartEvent;
        internal event Action GameEndEvent;
        internal event Action SaveEvent;

        internal event Action<GameStateID> StateEnterEvent;
        internal event Action<GameStateID> StateExitEvent;
        
        internal FSMProcessor<GameStateID, GameState> fsm;

        GameState currentState;
        GameState lastState;

        void Start()
        {
            itemsToInject.Add(config);
            itemsToInject.Add(new GameData());

            systems = FindObjectsOfType<GameSystem>().ToDictionary(x => x.GetType(), x => x);

            ProcessInstallers();

            GameData = itemsToInject.First(x => x.GetType() == typeof(GameData)) as GameData;
            PlayerData = itemsToInject.First(x => x.GetType() == typeof(PlayerData)) as PlayerData;

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

        public void GameRestart(int sceneIndex)
        {
            GameEndEvent?.Invoke();
            SaveGame();
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Saves all changes in Player Data to PlayerPrefs.
        /// On level complete\fail use GameRestart() instead.
        /// </summary>
        public void SaveGame()
        {
            SaveEvent?.Invoke();
        }

        public void ChangeGameState(GameStateID id)
        {
            if (currentState != null)
            {
                StateExitEvent?.Invoke(currentState.ID);
                lastState = currentState;
            }

            fsm.ChangeState(id);
            StateEnterEvent?.Invoke(id);
            currentState = fsm.CurrentState;
        }

        public T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        public GameStateID GetCurrentGamestateID()
        {
            return currentState.ID;
        }

        public GameStateID GetLastGamestateID()
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