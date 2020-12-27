using UnityEngine;
using Kuhpik.Pooling;
using NaughtyAttributes;

namespace Kuhpik
{
    public class PoolInstaller : MonoBehaviour
    {
        [SerializeField] bool usePooling;
        [SerializeField] [ShowIf("usePooling")] PoolingConfig config;

        public void Init()
        {
            if (usePooling)
                PoolingSystem.Init(config.Pools, config.Capacity);
        }
    }
}
