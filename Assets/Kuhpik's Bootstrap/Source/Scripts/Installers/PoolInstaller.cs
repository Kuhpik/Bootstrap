using UnityEngine;
using Kuhpik.Pooling;
using NaughtyAttributes;

namespace Kuhpik
{
    [DefaultExecutionOrder(10)]
    public class PoolInstaller : MonoBehaviour
    {
        [SerializeField] bool usePooling;
        [SerializeField] [ShowIf("usePooling")] PoolingConfig config;

        void Start()
        {
            if (usePooling) PoolingSystem.Init(config.Pools, config.Capacity);
        }
    }
}
