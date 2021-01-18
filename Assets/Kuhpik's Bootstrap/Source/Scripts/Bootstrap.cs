using Kuhpik.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kuhpik
{
    [DefaultExecutionOrder(500)]
    public class Bootstrap : MonoBehaviour
    {
        const string saveKey = "saveKey";

        [SerializeField] GameConfig config;

        static string[] statesOrder;
        static PlayerData playerData;
        static FSMProcessor<GameState> fsm;
        static Dictionary<Type, GameSystem> systems;
        static EGamestate lastState;

        void Start()
        {
            InitSystems();
        }

        void Update()
        {
            if (fsm.State.IsInited)
            {
                for (int i = 0; i < fsm.State.UpdateSystems.Length; i++)
                {
                    fsm.State.UpdateSystems[i].OnUpdate();
                }
            }
        }

        void FixedUpdate()
        {
            if (fsm.State.IsInited)
            {
                for (int i = 0; i < fsm.State.FixedUpdateSystems.Length; i++)
                {
                    fsm.State.FixedUpdateSystems[i].OnFixedUpdate();
                }
            }
        }

        void OnApplicationQuit()
        {
            #if UNITY_EDITOR
            Debug.Log("Possible to get Pooling System Error. Ignore it. Editor Issues");
            #endif
        }

        public static void GameRestart(int sceneIndex)
        {
            foreach (var system in systems.Values)
            {
                (system as IGameSystem).PerformAction<IDisposing>();
            }

            SaveGame();
            PoolingSystem.Clear();
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Saves all changes in Player Data to PlayerPrefs
        /// </summary>
        public static void SaveGame()
        {
            SaveExtension.Save(playerData, saveKey);
        }

        public static void ChangeGameState(EGamestate type, bool openScreen = true)
        {
            lastState = GetCurrentGamestate();
            ChangeGameState(type.GetName(), openScreen);
        }

        public static T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        public static EGamestate GetCurrentGamestate()
        {
            return fsm.State.Type;
        }

        public static EGamestate GetLastGamestate()
        {
            return lastState;
        }

        static void ChangeGameState(string stateName, bool openScreen = true)
        {
            fsm.State.Deactivate();
            fsm.ChangeState(stateName);
            fsm.State.Activate(openScreen);
        }

        void InitSystems()
        {
            CreatePools();
            ResolveSystems();
            LoadPlayerData();
            HandleGameStates();
            HandleInjections();
            HandleCamerasFOV();
            ActivateStates();
        }

        void ResolveSystems()
        {
            systems = FindObjectsOfType<GameSystem>().ToDictionary(system => system.GetType(), system => system);
        }

        void HandleGameStates()
        {
            GetComponentInChildren<GameStateInstaller>().InstallGameStates(out fsm, out statesOrder);
        }

        void LoadPlayerData()
        {
            playerData = GetComponentInChildren<PlayerDataInstaller>().InstallData(saveKey);
        }

        void HandleInjections()
        {
            GetComponentInChildren<InjectionsInstaller>().Inject(systems.Values, config, playerData, new GameData());
        }

        void HandleCamerasFOV()
        {
            GetComponentInChildren<CameraInstaller>().Process();
        }

        void CreatePools()
        {
            GetComponentInChildren<PoolInstaller>().Init();
        }

        void ActivateStates()
        {
            fsm.State.Activate(true);

            for (int i = 1; i < statesOrder.Length; i++)
            {
                ChangeGameState(statesOrder[i]);
            }
        }
    }
}