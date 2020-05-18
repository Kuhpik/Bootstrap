using UnityEngine;

namespace Kuhpik.Example
{
    public class ExampleSystem : GameSystem, IIniting, IRunning
    {
        [SerializeField] private Vector3 rotation = new Vector3(0, 5, 0);
        private Transform cube;

        void IIniting.OnInit()
        {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        }

        void IRunning.OnRun()
        {
            cube.Rotate(rotation * Time.deltaTime);
        }
    }
}