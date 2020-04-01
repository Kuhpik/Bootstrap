using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Kuhpik
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private UIScreenType intialScreen;

        private static Image background;
        private static Dictionary<UIScreenType, UIScreen> uiScreens;

        private void Awake()
        {
            uiScreens = FindObjectsOfType<UIScreen>().ToDictionary(x => x.Type, x => x);
            background = backgroundImage;
            OpenScreen(intialScreen);
        }

        public static void OpenScreen(UIScreenType type)
        {
            foreach (var screen in uiScreens.Values)
            {
                screen.Close();
            }

            uiScreens[type].Open();
            background.gameObject.SetActive(uiScreens[type].UseBackground);
            background.color = uiScreens[type].BackgroundColor;
        }

        public static void OpenScreenAdditionaly(UIScreenType type)
        {
            uiScreens[type].Open();
        }

        public static void CloseScreen(UIScreenType type)
        {
            uiScreens[type].Close();
        }

        public static T GetUIScreen<T>(UIScreenType type) where T : UIScreen
        {
            return uiScreens[type] as T;
        }
    }
}