using UnityEngine;

namespace Kuhpik
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        void Awake()
        {
            foreach (var instance in FindObjectsOfType(typeof(T), false))
            {
                if (instance != this)
                {
                    var go = (instance as T).gameObject;
                    WarnAboutDuplicate(go);
                    DestroyImmediate(go);
                }
            }

            Instance = this as T;
        }

        // Just adding a bit more safety
        // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationQuit.html
        void OnApplicationQuit()
        {
            Instance = null;
        }

        void WarnAboutDuplicate(GameObject instance)
        {
            Debug.LogError
            (
                $"Found duplicate Singleton in scene hierarchy. " +
                $"Type of: {typeof(T).Name} " +
                $"and GO name: {instance.name}!"
            );
        }
    }
}