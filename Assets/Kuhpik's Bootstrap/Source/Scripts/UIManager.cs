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
        [SerializeField] [BoxGroup("Settings")] Image backgroundScreen;
        [SerializeField] [BoxGroup("Settings")] EGamestate previewScreen;

        static Dictionary<EGamestate, UIScreen> stateScreens;
        static Dictionary<Type, UIScreen> uiScreens;
        static Image background;

        #region Editor

        [Button]
        void ShowPreview()
        {
            var screens = FindObjectsOfType<UIScreen>();
            var screen = screens.First(x => x.Type == previewScreen);

            HidePreview(screens);
            screen.Open();
        }

        [Button]
        void HidePreview()
        {
            var screens = FindObjectsOfType<UIScreen>();
            HidePreview(screens);
        }

        void HidePreview(UIScreen[] screens)
        {
            foreach (var screen in screens) screen.Close();
        }

        #endregion

        void Awake()
        {
            stateScreens = GameObject.FindObjectsOfType<UIScreen>().ToDictionary(x => x.Type, x => x);
            uiScreens = stateScreens.Values.Where(x => x.GetType() != typeof(UIScreen)).ToDictionary(x => x.GetType(), x => x);

            foreach (var screen in stateScreens.Values) screen.Subscribe();
            background = backgroundScreen;
        }

        public static void ChangeBackground(Sprite sprite)
        {
            ChangeBackground(sprite, Color.white);
        }

        public static void ChangeBackground(Sprite sprite, Color color)
        {
            background.sprite = sprite;
            background.color = color;
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
                ChangeBackground(stateScreens[type].BackgroundSprite, stateScreens[type].BackgroundColor);
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