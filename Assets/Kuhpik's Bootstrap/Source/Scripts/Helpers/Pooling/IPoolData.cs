using UnityEngine;

namespace Kuhpik.Pooling
{
    public interface IPoolData
    {
        int Capacity { get; }
        float PoolTime { get; }
        bool DontDestroy { get; }
        GameObject Prefab { get; }
        Component[] Components { get; }
    }
}