using UnityEngine;

namespace Kuhpik
{
    public abstract class DontDestroySingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }

            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}