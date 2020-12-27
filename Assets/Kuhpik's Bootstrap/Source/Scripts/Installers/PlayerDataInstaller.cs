using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    class PlayerDataInstaller : MonoBehaviour
    {
        [SerializeField] bool isTesting;
        [SerializeField] [ShowIf("isTesting")] PlayerData playerData;

        public PlayerData InstallData(string saveKey)
        {
            #if UNITY_EDITOR
            if (isTesting) return playerData;
            else return SaveExtension.Override(saveKey, new PlayerData());
            #else
            return SaveExtension.Override(saveKey, new PlayerData());
            #endif
        }
    }
}
