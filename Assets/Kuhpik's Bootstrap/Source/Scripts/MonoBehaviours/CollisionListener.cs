using System;
using UnityEngine;

namespace Kuhpik
{
    public class CollisionListener : MonoBehaviour
    {
        public event Action<Transform> CollisionEnterEvent, CollisionExitEvent;
        public event Action<Transform> TriggerEnterEvent, TriggetExitEvent;

        void OnCollisionEnter(Collision collision)
        {
            CollisionEnterEvent?.Invoke(collision.transform);
        }

        void OnCollisionExit(Collision collision)
        {
            CollisionExitEvent?.Invoke(collision.transform);
        }

        void OnTriggerEnter(Collider other)
        {
            TriggerEnterEvent?.Invoke(other.transform);
        }

        void OnTriggerExit(Collider other)
        {
            TriggetExitEvent?.Invoke(other.transform);
        }
    }
}