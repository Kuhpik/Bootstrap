using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kuhpik.Pooling
{
    internal static class PoolingSystem
    {
        #region DATA

        internal struct ObjectData
        {
            public GameObject gameObject;
            public Dictionary<Type, Component> components;
        }

        #endregion

        #region FIELDS

        private static int baseCapacity = 16;

        private static Dictionary<string, Queue<ObjectData>> pool, dontDestroyPool;
        private static Dictionary<string, Queue<ObjectData>> busy, dontDestroyBusy;
        private static Dictionary<string, IPoolData> poolingData, dontDestroyPoolingData;

        #endregion

        #region SETUP

        public static void Init(IList<IPoolData> datas, int capacity = 0)
        {
            if (capacity > 0) baseCapacity = capacity;

            foreach (var data in datas)
            {
                InitPool(data);
            }
        }

        public static void Clear()
        {
            poolingData = new Dictionary<string, IPoolData>();
            busy = new Dictionary<string, Queue<ObjectData>>();
            pool = new Dictionary<string, Queue<ObjectData>>();
        }

        #endregion

        #region ADD

        public static void InitPool(IPoolData data)
        {
            var dictionary = data.DontDestroy ? dontDestroyPoolingData : poolingData;

            if (!dictionary.ContainsKey(data.Prefab.name))
            {
                dictionary.Add(data.Prefab.name, data);
                CreatePool(data);
            }
        }

        public static void CreatePool(IPoolData data)
        {
            var dictionary = data.DontDestroy ? dontDestroyPool : pool;

            if (!dictionary.ContainsKey(data.Prefab.name))
            {
                var count = data.Capacity > 0 ? data.Capacity : baseCapacity;
                dictionary.Add(data.Prefab.name, new Queue<ObjectData>());

                for (int i = 0; i < count; i++)
                {
                    ExtendPool(GameObject.Instantiate(data.Prefab));
                }
            }
        }

        public static void ExtendPool(GameObject @object)
        {
            var data = poolingData.ContainsKey(@object.name) ? poolingData[@object.name] : dontDestroyPoolingData[@object.name];
            var pool = data.DontDestroy ? dontDestroyPool : PoolingSystem.pool;
            var objectData = new ObjectData();

            objectData.components = data.Components.ToDictionary(x => x.GetType(), x => x);
            objectData.gameObject = @object;

            pool[@object.name].Enqueue(objectData);
        }

        #endregion

        #region GET

        public static GameObject GetObject(GameObject original)
        {
            if (!CheckQueue(original, out var @object)) ExtendPool(GameObject.Instantiate(original));
            return Get(original);
        }

        public static GameObject GetObject(GameObject original, Transform parent, bool worldPositionStays = false)
        {
            if (!CheckQueue(original, out var @object)) ExtendPool(GameObject.Instantiate(original, parent, worldPositionStays));
            return Get(original);
        }
        
        public static GameObject GetObject(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!CheckQueue(original, out var @object)) ExtendPool(GameObject.Instantiate(original, position, rotation, parent));
            return Get(original);
        }
        
        public static GameObject GetObject(GameObject original, Vector3 position, Quaternion rotation)
        {
            if (!CheckQueue(original, out var @object)) ExtendPool(GameObject.Instantiate(original, position, rotation));
            return Get(original);
        }

        static bool CheckQueue(GameObject original, out GameObject @object)
        {
            var queue = pool[original.name];
            var exist = queue.Count > 0;

            @object = exist ? queue.Dequeue().gameObject : null;
            return exist;
        }

        static GameObject Get(GameObject @object)
        {
            return new GameObject();
        }

        //public static T GetComponent<T>(GameObject original) where T : Component
        //{
        //    var queue = pool[original.name];
        //}

        #endregion
    }
}