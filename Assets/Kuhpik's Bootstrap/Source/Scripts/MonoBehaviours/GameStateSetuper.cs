using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    internal class GameStateSetuper : MonoBehaviour
    {
        [SerializeField] EGamestate type;
        [SerializeField] bool isRestarting;
        [SerializeField] bool openAdditionalScreens;
        [SerializeField] [ReorderableList] [ShowIf("openAdditionalScreens")] EGamestate[] additionalScreens;
        [SerializeField] [ReorderableList] EGamestate[] allowedTransitions;

        public EGamestate Type => type;
        public bool IsRestarting => isRestarting;
        public bool OpenAdditionalScreens => openAdditionalScreens;
        public EGamestate[] AdditionalScreens => additionalScreens;
        public EGamestate[] AllowedTransitions => allowedTransitions;
    }
}