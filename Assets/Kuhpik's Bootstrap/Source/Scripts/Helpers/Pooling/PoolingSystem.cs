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
        private static Dictionary<string, Transform> objectGroupDictinary = new Dictionary<string, Transform>();
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
            foreach (var item in dataDictionary.ToArray())
            {
                if (!item.Value.DontDestroy)
                {
                    poolDictionary.Remove(item.Key);
                    dataDictionary.Remove(item.Key);
                    objectGroupDictinary.Remove(item.Key);
                }

                else
                {
                    PoolEverything(item.Value.Prefab);
                }
            }

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

        /// <summary>
        /// Call this if you want to init pool from code.
        /// Caching components is not an option here.
        /// </summary>
        public static void InitPool(GameObject prefab, int cout, bool dontDestyroy, float poolBackTime = 0f)
        {
            if (!dataDictionary.ContainsKey(prefab.name))
            {
                var poolData = new PoolingData(prefab, cout, dontDestyroy, poolBackTime);
                dataDictionary.Add(prefab.name, poolData);
                CreatePool(poolData);
            }
        }

        static void CreatePool(IPoolData data)
        {
            if (!poolDictionary.ContainsKey(data.Prefab.name))
            {
                var count = data.Capacity > 0 ? data.Capacity : baseCapacity;
                poolDictionary.Add(data.Prefab.name, new Queue<ObjectData>());

                objectGroupDictinary.Add(data.Prefab.name, new GameObject($"[POOL] {data.Prefab.name} group").transform);
                if (data.DontDestroy) GameObject.DontDestroyOnLoad(objectGroupDictinary[data.Prefab.name].gameObject);

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

            if (data.DontDestroy) GameObject.DontDestroyOnLoad(copy);

            copy.name = name;
            objectData.gameObject = copy;
            objectData.components = data.Components.ToDictionary(x => x.GetType(), x => copy.GetComponent(x.GetType()));

            PoolEnqueue(objectData);
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

        /// <summary>
        /// Pools object back using it's name (instances have same name as a prefab)
        /// </summary>
        public static void Pool(GameObject @object)
        {
            var data = busyDictionary[@object.name][@object.GetInstanceID()];
            busyDictionary[@object.name].Remove(@object.GetInstanceID());
            PoolEnqueue(data);
        }

        public async static void Pool(GameObject @object, float time)
        {
            await Task.Delay(TimeSpan.FromSeconds(time));
            Pool(@object);
        }

        /// <summary>
        /// Pools all active objects back using it's name (instances have same name as a prefab)
        /// </summary>
        public static void PoolEverything(GameObject @object)
        {
            if (!busyDictionary.ContainsKey(@object.name)) return;
            if (!busyDictionary[@object.name].Any()) return;

            foreach (var item in busyDictionary[@object.name].ToArray())
            {
                Pool(item.Value.gameObject);
            }
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
            data.gameObject.transform.SetParent(null);
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

        static void PoolEnqueue(ObjectData data)
        {
            data.gameObject.transform.SetParent(objectGroupDictinary[data.gameObject.name]);
            poolDictionary[data.gameObject.name].Enqueue(data);
            data.gameObject.SetActive(false);
        }

        #endregion
    }
}