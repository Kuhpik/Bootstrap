using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik
{
    internal static class PoolingSystem
    {
        #region Structs

        internal struct PoolingData
        {
            public int capacity;
            public float poolTime;
            public bool dontDestroy;
            public GameObject prefab;
            public List<Type> cachedComponents;
        }

        internal struct ObjectData
        {
            public GameObject gameObject;
            public Dictionary<Type, Component> components;
        }

        #endregion

        #region Fields

        private const int baseCapacity = 16;
        private static Dictionary<string, PoolingData> poolingData, dontDestroyPoolingData; //Used in pool creation;
        private static Dictionary<string, Queue<ObjectData>> pool, dontDestroyPool; //Pool itself;
        private static Dictionary<string, Queue<ObjectData>> busy, dontDestroyBusy; //Stores ObjectData that was taken from pool;

        #endregion

        #region Get

        public static GameObject GetObject(GameObject original)
        {
            if (CheckQueue(original, out var @object)) return @object;
            else return ExtendPool(GameObject.Instantiate(original));
        }

        public static GameObject GetObject(GameObject original, Transform parent, bool worldPositionStays = false)
        {
            if (CheckQueue(original, out var @object)) return @object;
            else return ExtendPool(GameObject.Instantiate(original, parent, worldPositionStays));
        }
        
        public static GameObject GetObject(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (CheckQueue(original, out var @object)) return @object;
            else return ExtendPool(GameObject.Instantiate(original, position, rotation, parent));
        }
        
        public static GameObject GetObject(GameObject original, Vector3 position, Quaternion rotation)
        {
            if (CheckQueue(original, out var @object)) return @object;
            else return ExtendPool(GameObject.Instantiate(original, position, rotation));
        }

        static bool CheckQueue(GameObject original, out GameObject @object)
        {
            var queue = pool[original.name];
            var exist = queue.Count > 0;

            @object = exist ? queue.Dequeue().gameObject : null;
            return exist;
        }

        static GameObject ExtendPool(GameObject @object)
        {
            return @object;
        }

        //public static T GetComponent<T>(GameObject original) where T : Component
        //{
        //    var queue = pool[original.name];
        //}

        #endregion

        #region Setup

        public static void Clear()
        {
            poolingData = new Dictionary<string, PoolingData>();
            busy = new Dictionary<string, Queue<ObjectData>>();
            pool = new Dictionary<string, Queue<ObjectData>>();
        }

        #endregion
    }
}