using UnityEngine;

namespace Kuhpik
{
    public abstract class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }

            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}