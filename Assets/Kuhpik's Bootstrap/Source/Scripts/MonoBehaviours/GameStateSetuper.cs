using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    internal class GameStateSetuper : MonoBehaviour
    {
        [SerializeField] private EGamestate type;
        [SerializeField] private bool isRestarting;
        [SerializeField] private bool openAdditionalScreens;
        [SerializeField] [ReorderableList] [ShowIf("openAdditionalScreens")] EGamestate[] additionalScreens;
        [SerializeField] [ReorderableList] private EGamestate[] allowedTransitions;

        public EGamestate Type => type;
        public bool IsRestarting => isRestarting;
        public bool OpenAdditionalScreens => openAdditionalScreens;
        public EGamestate[] AdditionalScreens => additionalScreens;
        public EGamestate[] AllowedTransitions => allowedTransitions;
    }
}