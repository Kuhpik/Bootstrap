using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    class PlayerDataInstaller : MonoBehaviour
    {
        [SerializeField] private bool isTesting;
        [SerializeField] [ShowIf("isTesting")] private PlayerData playerData;

        public PlayerData InstallData(string saveKey)
        {
            #if UNITY_EDITOR
            if (isTesting) return playerData;
            else return SaveExtension.Load(saveKey, new PlayerData());
            #else
            return SaveExtension.Load(saveKey, new PlayerData());
            #endif
        }
    }
}
