using Kuhpik.Pooling;
using UnityEngine;

namespace Kuhpik
{
    public class PoolingData : IPoolData
    {
        public int Capacity => capacity;
        public float PoolTime => poolTime;
        public GameObject Prefab => prefab;
        public bool DontDestroy => dontDestroy;
        public Component[] Components => components;

        int capacity;
        float poolTime;
        bool dontDestroy;
        GameObject prefab;
        Component[] components;

        public PoolingData(GameObject prefab, int capacity, bool dontDestroy, float poolTime = 0f)
        {
            this.capacity = capacity;
            this.poolTime = poolTime;
            this.dontDestroy = dontDestroy;
            this.prefab = prefab;
            this.components = new Component[0];
        }
    }
}