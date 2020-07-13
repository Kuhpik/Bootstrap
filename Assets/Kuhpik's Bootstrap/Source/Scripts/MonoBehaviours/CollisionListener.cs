using System;
using UnityEngine;

namespace Kuhpik
{
    public class CollisionListener : MonoBehaviour
    {
        public Action<Transform> CollisionEnterEvent, CollisionExitEvent;
        public Action<Transform> TriggerEnterEvent, TriggetExitEvent;

        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnterEvent?.Invoke(collision.transform);
        }

        private void OnCollisionExit(Collision collision)
        {
            CollisionExitEvent?.Invoke(collision.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnterEvent?.Invoke(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggetExitEvent?.Invoke(other.transform);
        }
    }
}