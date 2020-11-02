using Kuhpik.OdinSerializer;
using System;
using UnityEngine;

namespace Kuhpik
{
    public static class SaveExtension
    {
        /// <summary>
        /// Save value in PlayerPrefs using JsonUtility.
        /// </summary>
        public static void Save<T>(T value, string id)
        {
            byte[] bytes = SerializationUtility.SerializeValue(value, DataFormat.Binary);
            PlayerPrefs.SetString(id, Convert.ToBase64String(bytes));
        }

        /// <summary>
        /// Load value into ref field from PlayerPrefs using JsonUtility.
        /// </summary>
        public static T Load<T>(ref T field, string id, T defaultValue)
        {
            if (PlayerPrefs.HasKey(id))
            {
                byte[] bytes = Convert.FromBase64String(PlayerPrefs.GetString(id));
                field = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
            }

            else
            {
                field = defaultValue;
            }

            return field;
        }

        /// <summary>
        /// Load value from PlayerPrefs using JsonUtility.
        /// </summary>
        public static T Load<T>(string id, T defaultValue)
        {
            if (PlayerPrefs.HasKey(id))
            {
                byte[] bytes = Convert.FromBase64String(PlayerPrefs.GetString(id));
                return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
            }

            else
            {
                return defaultValue;
            }
        }
    }
}