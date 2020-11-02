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
        private const string saveKey = "saveKey";

        [SerializeField] private GameConfig config;

        private static string[] statesOrder;
        private static PlayerData playerData;
        private static FSMProcessor<GameState> fsm;
        private static Dictionary<Type, GameSystem> systems;

        private void Start()
        {
            InitSystems();
        }

        private void Update()
        {
            if (fsm.State.IsInited)
            {
                for (int i = 0; i < fsm.State.UpdateSystems.Length; i++)
                {
                    fsm.State.UpdateSystems[i].OnUpdate();
                }
            }
        }

        private void FixedUpdate()
        {
            if (fsm.State.IsInited)
            {
                for (int i = 0; i < fsm.State.FixedUpdateSystems.Length; i++)
                {
                    fsm.State.FixedUpdateSystems[i].OnFixedUpdate();
                }
            }
        }

        public static void GameRestart(int sceneIndex)
        {
            foreach (var system in systems.Values)
            {
                (system as IGameSystem).PerformAction<IDisposing>();
            }

            SaveExtension.Save(playerData, saveKey);
            SceneManager.LoadScene(sceneIndex);
            PoolingSystem.Clear();
        }

        public static void ChangeGameState(EGamestate type, bool openScreen = true)
        {
            ChangeGameState(type.GetName(), openScreen);
        }

        private static void ChangeGameState(string stateName, bool openScreen = true)
        {
            fsm.State.Deactivate();
            fsm.ChangeState(stateName);
            fsm.State.Activate(openScreen);
        }

        public static T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        private void InitSystems()
        {
            CreatePools();
            ResolveSystems();
            LoadPlayerData();
            HandleGameStates();
            HandleInjections();
            HandleCamerasFOV();
            ActivateStates();
        }

        private void ResolveSystems()
        {
            systems = FindObjectsOfType<GameSystem>().ToDictionary(system => system.GetType(), system => system);
        }

        private void HandleGameStates()
        {
            GetComponentInChildren<GameStateInstaller>().InstallGameStates(out fsm, out statesOrder);
        }

        private void LoadPlayerData()
        {
            playerData = GetComponentInChildren<PlayerDataInstaller>().InstallData(saveKey);
        }

        private void HandleInjections()
        {
            GetComponentInChildren<InjectionsInstaller>().Inject(systems.Values, config, playerData, new GameData());
        }

        private void HandleCamerasFOV()
        {
            GetComponentInChildren<CameraInstaller>().Process();
        }

        private void CreatePools()
        {
            GetComponentInChildren<PoolInstaller>().Init();
        }

        private void ActivateStates()
        {
            fsm.State.Activate(true);

            for (int i = 1; i < statesOrder.Length; i++)
            {
                ChangeGameState(statesOrder[i]);
            }
        }
    }
}