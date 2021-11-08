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
        [SerializeField] [BoxGroup("Settings")] GameStateID previewScreen;

        static Dictionary<GameStateID, UIScreen> stateScreens;
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

        public static void OpenScreen(GameStateID id)
        {
            foreach (var screen in stateScreens.Values)
            {
                screen.Close();
            }

            if (stateScreens.ContainsKey(id))
            {
                stateScreens[id].Open();
                background.gameObject.SetActive(stateScreens[id].UseBackground);
                ChangeBackground(stateScreens[id].BackgroundSprite, stateScreens[id].BackgroundColor);
            }
        }

        public static void OpenScreenAdditionaly(GameStateID id)
        {
            stateScreens[id].Open();
        }

        public static void CloseScreen(GameStateID id)
        {
            stateScreens[id].Close();
        }

        public static UIScreen GetUIScreen(GameStateID id)
        {
            return stateScreens[id];
        }

        public static T GetUIScreen<T>() where T : UIScreen
        {
            return uiScreens[typeof(T)] as T;
        }
    }
}