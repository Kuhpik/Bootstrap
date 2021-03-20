using UnityEngine;
using Kuhpik.Pooling;
using NaughtyAttributes;

namespace Kuhpik
{
    [DefaultExecutionOrder(10)]
    public class PoolInstaller : MonoBehaviour
    {
        [SerializeField] bool usePooling;
        [SerializeField] [ShowIf("usePooling")] int baseCapacity;
        [SerializeField] [ShowIf("usePooling")] string loadingPath = "Pooling";

        void Start()
        {
            if (usePooling)
            {
                var pools = Resources.LoadAll<Pool>(loadingPath);
                PoolingSystem.Init(pools, baseCapacity);
            }
        }
    }
}
