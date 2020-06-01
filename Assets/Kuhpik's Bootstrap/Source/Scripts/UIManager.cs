using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using NaughtyAttributes;

namespace Kuhpik
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] [BoxGroup("Settings")] private Image backgroundScreen;
        [SerializeField] [BoxGroup("Settings")] private EGamestate previewScreen;

        private static Image background;
        private static Dictionary<EGamestate, UIScreen> stateScreens;
        private static Dictionary<Type, UIScreen> uiScreens;

        #region Editor

        [Button]
        private void ShowPreview()
        {
            var screens = FindObjectsOfType<UIScreen>();
            var screen = screens.First(x => x.Type == previewScreen);
            screen.Open();
        }

        [Button]
        private void HidePreview()
        {
            var screens = FindObjectsOfType<UIScreen>();
            foreach (var screen in screens) screen.Close();
        }

        #endregion

        private void Awake()
        {
            stateScreens = GameObject.FindObjectsOfType<UIScreen>().ToDictionary(x => x.Type, x => x);
            uiScreens = stateScreens.Values.ToDictionary(x => x.GetType(), x => x);

            foreach (var screen in stateScreens.Values) screen.Subscribe();
            background = backgroundScreen;
        }

        public static void OpenScreen(EGamestate type)
        {
            foreach (var screen in stateScreens.Values)
            {
                screen.Close();
            }

            if (stateScreens.ContainsKey(type))
            {
                stateScreens[type].Open();
                background.gameObject.SetActive(stateScreens[type].UseBackground);
                background.color = stateScreens[type].BackgroundColor;
            }
        }

        public static void OpenScreenAdditionaly(EGamestate type)
        {
            stateScreens[type].Open();
        }

        public static void CloseScreen(EGamestate type)
        {
            stateScreens[type].Close();
        }

        public static UIScreen GetUIScreen(EGamestate type)
        {
            return stateScreens[type];
        }

        public static T GetUIScreen<T>() where T : UIScreen
        {
            return uiScreens[typeof(T)] as T;
        }
    }
}