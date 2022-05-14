using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik
{
    public class PlayerDataInstaller : Installer
    {
        [SerializeField] bool isTesting;
        [SerializeField] [ShowIf("isTesting")] PlayerData testData;

        public override int Order => 2;

        const string saveKey = "saveKey";
        PlayerData data;

        public override void Process()
        {
            data = HandlePlayerData();

            Bootstrap.Instance.itemsToInject.Add(data);
            Bootstrap.Instance.SaveEvent += Save;
        }

        PlayerData HandlePlayerData()
        {
#if UNITY_EDITOR
            return isTesting ? testData : Load();
#else
            return Load();
#endif
        }

        void Save()
        {
            SaveExtension.Save(data, saveKey);
        }

        PlayerData Load()
        {
            return SaveExtension.Load(saveKey, new PlayerData());
        }
    }
}
