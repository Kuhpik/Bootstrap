using UnityEngine;

namespace Kuhpik.Extensions
{
    public static class LogExtensions
    {
        public static void Log(string message)
        {
            Log(message, Color.white);
        }

        public static void Log(string message, Color color)
        {
#if DEBUG
            Debug.Log($"<color={ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
#endif
        }
    }
}