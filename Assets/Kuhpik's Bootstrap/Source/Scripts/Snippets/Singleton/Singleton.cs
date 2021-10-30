using UnityEngine;

namespace Kuhpik
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }

            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}