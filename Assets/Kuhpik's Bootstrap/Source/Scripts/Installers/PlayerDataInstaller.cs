using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    [DefaultExecutionOrder(10)]
    public class PlayerDataInstaller : MonoBehaviour
    {
        [SerializeField] bool isTesting;
        [SerializeField] [ShowIf("isTesting")] PlayerData playerData;

        const string saveKey = "saveKey";
        PlayerData data;

        void Start()
        {
            data = HandlePlayerData();

            Bootstrap.itemsToInject.Add(data);
            Bootstrap.OnSaveEvent += Save;
        }

        PlayerData HandlePlayerData()
        {
            #if UNITY_EDITOR
            if (isTesting) return playerData;
            else return SaveExtension.Override(saveKey, new PlayerData());
            #else
            return SaveExtension.Override(saveKey, new PlayerData());
            #endif
        }

        void Save()
        {
            SaveExtension.Save(data, saveKey);
        }
    }
}
