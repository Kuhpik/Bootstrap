using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    public class UIScreen : MonoBehaviour, IUIScreen
    {
        [SerializeField] [BoxGroup("Settings")] EGamestate type;
        [SerializeField] [BoxGroup("Settings")] bool getScreenFromChild = true;
        [SerializeField] [BoxGroup("Settings")] [HideIf("getScreenFromChild")] GameObject screen;

        [SerializeField] [BoxGroup("Background")] bool useBackground;
        [SerializeField] [BoxGroup("Background")] [ShowIf("useBackground")] Color backgroundColor;
        [SerializeField] [BoxGroup("Background")] [ShowIf("useBackground")] Sprite backgroundSprite;

        //You will get the idea once you use it
        [SerializeField] [BoxGroup("Elements")] bool hideElementsOnOpen;
        [SerializeField] [BoxGroup("Elements")] bool showElementsOnHide;

        [SerializeField] [BoxGroup("Elements")] [ShowIf("hideElementsOnOpen")] GameObject[] elementsToHideOnOpen;
        [SerializeField] [BoxGroup("Elements")] [ShowIf("showElementsOnHide")] GameObject[] elementsToShowOnHide;

        public EGamestate Type => type;
        public bool UseBackground => useBackground;
        public Color BackgroundColor => backgroundColor;
        public Sprite BackgroundSprite => backgroundSprite;

        void Awake()
        {
            screen = getScreenFromChild ? transform.GetChild(0).gameObject : screen;
        }

        public virtual void Open()
        {
            foreach (var element in elementsToHideOnOpen)
            {
                element.SetActive(false);
            }

            screen.SetActive(true);
        }

        public virtual void Close()
        {
            foreach (var element in elementsToShowOnHide)
            {
                element.SetActive(true);
            }

            screen.SetActive(false);
        }

        /// <summary>
        /// Use it for special cases.
        /// </summary>
        public virtual void Refresh()
        {
        }

        /// <summary>
        /// Subscribe is called on Awake()
        /// </summary>
        public virtual void Subscribe()
        {
        }
    }
}