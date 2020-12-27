using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik.Pooling
{
    [CreateAssetMenu(menuName = "Kuhpik/Pooling/Config")]
    public class PoolingConfig : ScriptableObject
    {
        [SerializeField] int defaultCapacity = 16;
        [SerializeField] [ReorderableList] Pool[] pools;

        public int Capacity => defaultCapacity;
        public Pool[] Pools => pools;
    }
}