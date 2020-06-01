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

        /// <summary>
        /// Load value into ref field from PlayerPrefs using JsonUtility.
        /// </summary>
        public static T Load<T>(ref T field, string id, T defaultValue)
        {
            if (PlayerPrefs.HasKey(id))
            {
                var @string = PlayerPrefs.GetString(id);
                field = JsonUtility.FromJson<T>(@string);
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