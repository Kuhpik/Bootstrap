using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    internal class GameStateSetuper : MonoBehaviour
    {
        [SerializeField] GameStateName type;
        [SerializeField] bool isRestarting;
        [SerializeField] bool useAdditionalScreens;
        [SerializeField] bool useAdditionalStates;
        [SerializeField] [ReorderableList] GameStateName[] allowedTransitions;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] GameStateName[] additionalStatesInTheBegining;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalStates")] GameStateName[] additionalStatesInTheEnd;
        [SerializeField] [ReorderableList] [ShowIf("useAdditionalScreens")] GameStateName[] additionalScreens;

        public GameStateName Type => type;
        public bool IsRestarting => isRestarting;
        public bool UseAdditionalScreens => useAdditionalScreens;
        public bool UseAdditionalStates => useAdditionalStates;
        public GameStateName[] AdditionalStatesInTheBegining => additionalStatesInTheBegining;
        public GameStateName[] AdditionalStatesInTheEnd => additionalStatesInTheEnd;
        public GameStateName[] AdditionalScreens => additionalScreens;
        public GameStateName[] AllowedTransitions => allowedTransitions;
    }
}