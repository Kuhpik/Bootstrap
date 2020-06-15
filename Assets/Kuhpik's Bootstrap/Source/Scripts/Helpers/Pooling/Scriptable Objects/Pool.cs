using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik.Pooling
{
    [CreateAssetMenu(menuName = "Kuhpik/Pooling/Pool")]
    public class Pool : ScriptableObject, IPoolData
    {
        [SerializeField] [BoxGroup("Settings")] private int capacity;
        [SerializeField] [BoxGroup("Settings")] private float poolTime;
        [SerializeField] [BoxGroup("Settings")] private bool dontDestroy;
        [SerializeField] [BoxGroup("Settings")] private GameObject prefab;
        [SerializeField] [ReorderableList] private Component[] cachedComponents;

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