using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik.Pooling
{
    [CreateAssetMenu(menuName = "Game Asset/Pooling/Config")]
    public class PoolingConfig : ScriptableObject
    {
        [SerializeField] private int capacity;
        [SerializeField] [ReorderableList] private Pool[] pools;

        public int Capacity => capacity;
        public Pool[] Pools => pools;
    }
}