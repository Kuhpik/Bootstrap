using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik.Pooling
{
    [CreateAssetMenu(menuName = "Kuhpik/Pooling/Pool")]
    public class Pool : ScriptableObject, IPoolData
    {
        [SerializeField] [BoxGroup("Settings")] int capacity;
        [SerializeField] [BoxGroup("Settings")] float poolTime;
        [SerializeField] [BoxGroup("Settings")] bool dontDestroy;
        [SerializeField] [BoxGroup("Settings")] GameObject prefab;
        [SerializeField] [ReorderableList] Component[] cachedComponents;

        public int Capacity => capacity;
        public float PoolTime => poolTime;
        public bool DontDestroy => dontDestroy;
        public GameObject Prefab => prefab;
        public Component[] Components => cachedComponents;

        [Button]
        void GetComponents()
        {
            cachedComponents = prefab.GetComponents<Component>();
        }
    }
}