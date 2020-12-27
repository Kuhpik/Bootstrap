using System;
using UnityEngine;

namespace Kuhpik
{
    public class CollisionListener2D : MonoBehaviour
    {
        public event Action<Transform> CollisionEnterEvent2D, CollisionExitEvent2D;
        public event Action<Transform> TriggerEnterEvent2D, TriggetExitEvent2D;

        void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEnterEvent2D?.Invoke(collision.transform);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            CollisionExitEvent2D?.Invoke(collision.transform);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            TriggerEnterEvent2D?.Invoke(collision.transform);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            TriggetExitEvent2D?.Invoke(collision.transform);
        }
    }
}