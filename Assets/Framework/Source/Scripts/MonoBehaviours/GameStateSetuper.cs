using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace Kuhpik
{
    internal class GameStateSetuper : MonoBehaviour
    {
        [SerializeField] private EGamestate type;
        [SerializeField] private bool isRestarting;
        [SerializeField] [ReorderableList] private EGamestate[] allowedTransitions;

        public EGamestate Type => type;
        public bool IsRestarting => isRestarting;
        public string[] AllowedTransitions => allowedTransitions.Select(x => x.GetName()).ToArray();
    }
}