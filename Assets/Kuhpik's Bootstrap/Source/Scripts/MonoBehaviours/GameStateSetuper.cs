using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    internal class GameStateSetuper : MonoBehaviour
    {
        [SerializeField] EGamestate type;
        [SerializeField] bool isRestarting;
        [SerializeField] bool useAdditionalScreens;
        [SerializeField] bool useAdditionalStates;
        [SerializeField] [ReorderableList] EGamestate[] allowedTransitions;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] EGamestate[] additionalStatesInTheBegining;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] EGamestate[] additionalStatesInTheEnd;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalScreens")] EGamestate[] additionalScreens;

        public EGamestate Type => type;
        public bool IsRestarting => isRestarting;
        public bool UseAdditionalScreens => useAdditionalScreens;
        public bool UseAdditionalStates => useAdditionalStates;
        public EGamestate[] AdditionalStatesInTheBegining => additionalStatesInTheBegining;
        public EGamestate[] AdditionalStatesInTheEnd => additionalStatesInTheEnd;
        public EGamestate[] AdditionalScreens => additionalScreens;
        public EGamestate[] AllowedTransitions => allowedTransitions;
    }
}