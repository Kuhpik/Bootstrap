using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    internal class GameStateSetuper : MonoBehaviour
    {
        [SerializeField] private bool isRestarting;
        [SerializeField] [ReorderableList] private string[] allowedTransitions;

        public bool IsRestarting => isRestarting;
        public string[] AllowedTransitions => allowedTransitions;
    }
}