using Kuhpik.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik.Example
{
    public class ExampleSystem : GameSystem, IIniting, IUpdating, IFixedUpdating
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject dontDestroyprefab;
        [SerializeField] private Vector3 rotation = new Vector3(0, 5, 0);

        List<Transform> cubes;

        void IIniting.OnInit()
        {
            for (int i = 0; i < 512; i++)
            {
                PoolingSystem.GetComponent<Rigidbody>(dontDestroyprefab, out var cubeRB);
                cubeRB.transform.position = new Vector3(-7 + 2.5f * i, 0, 15);
                cubeRB.velocity = Vector3.zero;
                cubeRB.useGravity = i % 2 == 0;
            }

            cubes = new List<Transform>() { PoolingSystem.GetObject(prefab).transform };
        }

        void IUpdating.OnUpdate()
        {
            foreach (var cube in cubes)
            {
                cube.Rotate(rotation * Time.deltaTime);
            }

            Debug.Log("Update");
        }

        void IFixedUpdating.OnFixedUpdate()
        {
            Debug.Log("Fixed update");
        }
    }
}