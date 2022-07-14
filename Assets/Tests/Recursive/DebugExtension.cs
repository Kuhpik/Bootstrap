using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

namespace Kuhpik.Framework.Tests
{
    public static class DebugExtension
    {
        public static void DisplayTestMessage(this GameSystem system, string msg)
        {
            // Debug.Log($"Frame: {Time.frameCount}. {system.GetType().FullName} calls method: {msg}");
            // Debug.Log($"{system.GetType().FullName} calls method: {msg}");
            try
            {
                var state = Bootstrap.Instance.GetCurrentGamestateID();
                Debug.Log($"State: {state}. {system.GetType().FullName} calls method: {msg}");
            }
            catch (System.Exception e)
            {
                Debug.Log($"{ system.GetType().FullName } calls method: { msg }");
            }
        }
    }
}
