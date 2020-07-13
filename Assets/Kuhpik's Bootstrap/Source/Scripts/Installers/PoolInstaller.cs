using UnityEngine;
using Kuhpik.Pooling;
using NaughtyAttributes;

namespace Kuhpik
{
    public class PoolInstaller : MonoBehaviour
    {
        [SerializeField] private bool usePooling;
        [SerializeField] [ShowIf("usePooling")] private PoolingConfig config;

        public void Init()
        {
            if (usePooling)
                PoolingSystem.Init(config.Pools, config.Capacity);
        }
    }
}
