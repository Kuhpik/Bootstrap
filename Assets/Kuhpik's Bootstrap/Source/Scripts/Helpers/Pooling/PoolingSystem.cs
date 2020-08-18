using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Kuhpik.Pooling
{
    internal static class PoolingSystem
    {
        #region DATA

        internal class ObjectData
        {
            public GameObject gameObject;
            public Dictionary<Type, Component> components;
        }

        #endregion

        #region FIELDS

        private static int baseCapacity = 16;
        private static Dictionary<string, IPoolData> dataDictionary = new Dictionary<string, IPoolData>();
        private static Dictionary<string, Queue<ObjectData>> poolDictionary = new Dictionary<string, Queue<ObjectData>>();
        private static Dictionary<string, Dictionary<int, ObjectData>> busyDictionary = new Dictionary<string, Dictionary<int, ObjectData>>();

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

        /// <summary>
        /// Better call it when switching scenes.
        /// </summary>
        public static void Clear()
        {            
            foreach (var pair in busyDictionary.ToArray())
            {
                if (dataDictionary[pair.Key].DontDestroy)
                {
                    foreach (var data in pair.Value.Values.ToArray())
                    {
                        Pool(data.gameObject);
                    }
                }
            }

            poolDictionary = poolDictionary.Where(x => dataDictionary[x.Key].DontDestroy).ToDictionary(p => p.Key, p => p.Value);
            dataDictionary = dataDictionary.Where(x => x.Value.DontDestroy).ToDictionary(p => p.Key, p => p.Value);
            busyDictionary = new Dictionary<string, Dictionary<int, ObjectData>>();
        }

        #endregion

        #region ADD

        public static void InitPool(IPoolData data)
        {
            if (!dataDictionary.ContainsKey(data.Prefab.name))
            {
                dataDictionary.Add(data.Prefab.name, data);
                CreatePool(data);
            }
        }

        static void CreatePool(IPoolData data)
        {
            if (!poolDictionary.ContainsKey(data.Prefab.name))
            {
                var count = data.Capacity > 0 ? data.Capacity : baseCapacity;
                poolDictionary.Add(data.Prefab.name, new Queue<ObjectData>());

                for (int i = 0; i < count; i++)
                {
                    ExtendPool(data.Prefab);
                }
            }
        }

        static void ExtendPool(GameObject @object)
        {
            var copy = GameObject.Instantiate(@object);
            var name = @object.name;
            var data = dataDictionary[name];
            var objectData = new ObjectData();

            copy.name = name;
            copy.SetActive(false);
            if (data.DontDestroy) GameObject.DontDestroyOnLoad(copy);

            objectData.gameObject = copy;
            objectData.components = data.Components.ToDictionary(x => x.GetType(), x => copy.GetComponent(x.GetType()));

            poolDictionary[name].Enqueue(objectData);
        }

        #endregion

        #region GET OBJECT

        public static GameObject GetObject(GameObject @object)
        {
            var data = GetData(@object);
            return data.gameObject;
        }

        #endregion

        #region GET COMPONENT

        public static void GetComponent<T1>(GameObject @object, out T1 c1) where T1 : Component
        {
            var data = GetData(@object);
            c1 = data.components[typeof(T1)] as T1;
        }

        public static void GetComponent<T1, T2>(GameObject @object, out T1 c1, out T2 c2) where T1 : Component where T2 : Component
        {
            var data = GetData(@object);

            c1 = data.components[typeof(T1)] as T1;
            c2 = data.components[typeof(T2)] as T2;
        }

        public static void GetComponent<T1, T2, T3>(GameObject @object, out T1 c1, out T2 c2, out T3 c3) where T1 : Component where T2 : Component where T3 : Component
        {
            var data = GetData(@object);

            c1 = data.components[typeof(T1)] as T1;
            c2 = data.components[typeof(T2)] as T2;
            c3 = data.components[typeof(T3)] as T3;
        }

        #endregion

        #region POOL

        public static void Pool(GameObject @object)
        {
            var data = busyDictionary[@object.name][@object.GetInstanceID()];
            busyDictionary[@object.name].Remove(@object.GetInstanceID());
            poolDictionary[@object.name].Enqueue(data);
            data.gameObject.SetActive(false);

            if (dataDictionary[@object.name].DontDestroy)
            {
                data.gameObject.transform.SetParent(null);
            }
        }

        public async static void Pool(GameObject @object, float time)
        {
            await Task.Delay(TimeSpan.FromSeconds(time));
            Pool(@object);
        }

        #endregion

        #region HELPERS

        static ObjectData GetData(GameObject @object)
        {
            CheckQueue(@object);

            var data = poolDictionary[@object.name].Dequeue();
            if (!busyDictionary.ContainsKey(@object.name)) busyDictionary.Add(@object.name, new Dictionary<int, ObjectData>());
            if (dataDictionary[@object.name].PoolTime > 0) Pool(data.gameObject, dataDictionary[@object.name].PoolTime);
            busyDictionary[@object.name].Add(data.gameObject.GetInstanceID(), data);
            data.gameObject.SetActive(true);
            return data;
        }

        static void CheckQueue(GameObject @object)
        {
            var queue = poolDictionary[@object.name];

            if (queue.Count == 0)
            {
                ExtendPool(@object);
            }
        }

        #endregion
    }
}