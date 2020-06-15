using Kuhpik.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik.Example
{
    public class ExampleSystem : GameSystem, IIniting, IRunning
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector3 rotation = new Vector3(0, 5, 0);
        List<Transform> cubes;

        void IIniting.OnInit()
        {
            cubes = new List<Transform>();

            for (int i = 0; i < 7; i++)
            {
                PoolingSystem.GetComponent<Rigidbody>(prefab, out var cubeRB);
                cubeRB.transform.position = new Vector3(-7 + 2.5f * i, 0, 15);
                if (i % 2 == 0) cubeRB.useGravity = true;

                cubes.Add(cubeRB.transform);
            }
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