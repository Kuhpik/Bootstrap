using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    public class PlayerDataInstaller : Installer
    {
        [SerializeField] bool isTesting;
        [SerializeField] [ShowIf("isTesting")] PlayerData playerData;

        public override int Order => 2;

        const string saveKey = "saveKey";
        PlayerData data;

        public override void Process()
        {
            data = HandlePlayerData();

            Bootstrap.itemsToInject.Add(data);
            Bootstrap.SaveEvent += Save;
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
