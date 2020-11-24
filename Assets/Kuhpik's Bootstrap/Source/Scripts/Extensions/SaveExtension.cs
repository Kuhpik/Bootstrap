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
            var @string = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(id, @string);
        }

        public static T Override<T>(string id, T value)
        {
            if (PlayerPrefs.HasKey(id))
            {
                var @string = PlayerPrefs.GetString(id);
                JsonUtility.FromJsonOverwrite(@string, value);
                return value;
            }

            else
            {
                return value;
            }
        }

        /// <summary>
        /// Load value from PlayerPrefs using JsonUtility.
        /// </summary>
        public static T Load<T>(string id, T defaultValue)
        {
            if (PlayerPrefs.HasKey(id))
            {
                var @string = PlayerPrefs.GetString(id);
                return JsonUtility.FromJson<T>(@string);
            }

            else
            {
                return defaultValue;
            }
        }
    }
}