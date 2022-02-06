using Sirenix.OdinSerializer;
using System;
using UnityEngine;

namespace Kuhpik
{
    public static class SaveExtension
    {
        public static void Save<T>(T value, string key)
        {
            byte[] bytes = SerializationUtility.SerializeValue(value, DataFormat.Binary);
            PlayerPrefs.SetString(key, Convert.ToBase64String(bytes));
        }

        public static T Load<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                byte[] bytes = Convert.FromBase64String(PlayerPrefs.GetString(key));
                return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
            }

            else
            {
                return defaultValue;
            }
        }
    }
}