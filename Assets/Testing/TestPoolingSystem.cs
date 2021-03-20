using Kuhpik;
using Kuhpik.Pooling;
using UnityEngine;

public class TestPoolingSystem : GameSystem, IIniting, IUpdating
{
    [SerializeField] KeyCode restartKey;
    [SerializeField] bool isTesting;

    [Header("Pool")]
    [SerializeField] GameObject[] prefabsToGet;
    [SerializeField] GameObject[] prefabsToPool;
    [SerializeField] GameObject codeDontDestroyPrefab;

    void IIniting.OnInit()
    {
        if (!isTesting) return;

        foreach (var prefab in prefabsToGet)
        {
            PoolingSystem.GetObject(prefab);
        }

        foreach (var prefab in prefabsToPool)
        {
            var @object = PoolingSystem.GetObject(prefab);
            PoolingSystem.Pool(@object);
        }

        PoolingSystem.InitPool(codeDontDestroyPrefab, 3, true, 0);
        PoolingSystem.InitPool(new GameObject("Code"), 5, false);
    }

    void IUpdating.OnUpdate()
    {
        if (Input.GetKeyDown(restartKey))
        {
            Bootstrap.GameRestart(0);
        }
    }
}
