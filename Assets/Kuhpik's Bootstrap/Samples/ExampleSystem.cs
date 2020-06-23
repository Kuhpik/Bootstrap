using Kuhpik.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik.Example
{
    public class ExampleSystem : GameSystem, IIniting, IRunning
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject dontDestroyprefab;
        [SerializeField] private Vector3 rotation = new Vector3(0, 5, 0);

        List<Transform> cubes;

        void IIniting.OnInit()
        {
            cubes = new List<Transform>();

            for (int i = 0; i < 7; i++)
            {
                PoolingSystem.GetComponent<Rigidbody>(dontDestroyprefab, out var cubeRB);
                cubeRB.transform.position = new Vector3(-7 + 2.5f * i, 0, 15);
                cubeRB.velocity = Vector3.zero;
                cubeRB.useGravity = i % 2 == 0;

                cubes.Add(cubeRB.transform);
            }

            PoolingSystem.GetObject(prefab, new Vector3(0, 3, 10), Quaternion.identity);
        }

        void IRunning.OnRun()
        {
            foreach (var cube in cubes)
            {
                cube.Rotate(rotation * Time.deltaTime);
            }            
        }
    }
}