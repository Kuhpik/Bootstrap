using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kuhpik
{
    public static class PoolingSystem
    {
        private const int baseCapacity = 16;

        private static Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
        private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        /// <summary>
        /// Creates pool with specified id. You can also create pool automatically by using GetObject().
        /// </summary>
        public static void CreatePool(string id, GameObject prefab, int capacity = baseCapacity, bool dontDestroy = false)
        {
            prefabs.Add(id, prefab);

            var queue = new Queue<GameObject>();
            for (int i = 0; i < capacity; i++)
            {
                queue.Enqueue(InstantiateObject(id, false, dontDestroy));
            }

            pools.Add(id, queue);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it.
        /// </summary>
        public static GameObject GetObject(string id, GameObject prefab, int capacity = baseCapacity, float poolTime = 0f, bool dontDestroy = false)
        {
            if (!pools.ContainsKey(id)) CreatePool(id, prefab, capacity, dontDestroy);
            var @object = GetObject(id);

            if (poolTime > 0) PoolObject(@object, id, poolTime);
            return @object;
        }

        /// <summary>
        /// Get object from pool
        /// </summary>
        public static GameObject GetObject(string id)
        {
            var @object = pools[id].Count != 0 ? pools[id].Dequeue() : InstantiateObject(id);
            @object.SetActive(true);
            return @object;
        }

        #region CreatePool adapters

        /// <summary>
        /// Creates pool with specified id. You can also create pool automatically by using GetObject().
        /// </summary>
        public static void CreatePool(string id, GameObject prefab, bool dontDestroy = false)
        {
            CreatePool(id, prefab, baseCapacity, dontDestroy);
        }

        /// <summary>
        /// Creates pool. Uses gameobject's name as id. You can also create pool automatically by using GetObject().
        /// </summary>
        public static void CreatePool(GameObject prefab, int capacity = baseCapacity, bool dontDestroy = false)
        {
            CreatePool(prefab.name, prefab, capacity, dontDestroy);
        }

        /// <summary>
        /// Creates pool. Uses gameobject's name as id. You can also create pool automatically by using GetObject().
        /// </summary>
        public static void CreatePool(GameObject prefab, bool dontDestroy = false)
        {
            CreatePool(prefab.name, prefab, baseCapacity, dontDestroy);
        }

        #endregion CreatePool adapters

        #region GetObject adapters

        /// <summary>
        /// Get object from pool. If there is no pool - creates it.
        /// </summary>
        public static GameObject GetObject(string id, GameObject prefab, int capacity = baseCapacity, float poolTime = 0f)
        {
            return GetObject(id, prefab, capacity, poolTime, false);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it.
        /// </summary>
        public static GameObject GetObject(string id, GameObject prefab, int capacity = baseCapacity, bool dontDestroy = false)
        {
            return GetObject(id, prefab, capacity, 0f, dontDestroy);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it.
        /// </summary>
        public static GameObject GetObject(string id, GameObject prefab, float poolTime = 0f)
        {
            return GetObject(id, prefab, baseCapacity, poolTime, false);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it.
        /// </summary>
        public static GameObject GetObject(string id, GameObject prefab, bool dontDestroy = false)
        {
            return GetObject(id, prefab, baseCapacity, 0f, dontDestroy);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it. Uses gameobject's name as id.
        /// </summary>
        public static GameObject GetObject(GameObject prefab, int capacity = baseCapacity, float poolTime = 0f, bool dontDestroy = false)
        {
            return GetObject(prefab.name, prefab, capacity, 0f, dontDestroy);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it. Uses gameobject's name as id.
        /// </summary>
        public static GameObject GetObject(GameObject prefab, int capacity = baseCapacity, float poolTime = 0f)
        {
            return GetObject(prefab.name, prefab, capacity, poolTime, false);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it. Uses gameobject's name as id.
        /// </summary>
        public static GameObject GetObject(GameObject prefab, int capacity = baseCapacity, bool dontDestroy = false)
        {
            return GetObject(prefab.name, prefab, capacity, 0f, dontDestroy);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it. Uses gameobject's name as id.
        /// </summary>
        public static GameObject GetObject(GameObject prefab, float poolTime = 0f)
        {
            return GetObject(prefab.name, prefab, baseCapacity, poolTime, false);
        }

        /// <summary>
        /// Get object from pool. If there is no pool - creates it. Uses gameobject's name as id.
        /// </summary>
        public static GameObject GetObject(GameObject prefab, bool dontDestroy = false)
        {
            return GetObject(prefab.name, prefab, baseCapacity, 0f, dontDestroy);
        }

        /// <summary>
        /// Get object from pool. Uses gameobject's name as id.
        /// </summary>
        public static GameObject GetObject(GameObject prefab)
        {
            return GetObject(prefab.name);
        }

        #endregion GetObject with pool creation

        #region Pooling object

        /// <summary>
        /// Pool object back
        /// </summary>
        public static void PoolObject(GameObject @object, string id)
        {
            pools[id].Enqueue(@object);
            @object.SetActive(false);
        }

        /// <summary>
        /// Pool object back after some time (like Destroy with time param)
        /// </summary>
        public static async void PoolObject(GameObject @object, string id, float time)
        {
            await Task.Delay(TimeSpan.FromSeconds(time));
            PoolObject(@object, id);
        }

        /// <summary>
        /// Pool object back. Uses gameobject's name as id. Specify id if you changed the object's name.
        /// </summary>
        public static void PoolObject(GameObject @object)
        {
            PoolObject(@object, @object.name);
        }

        /// <summary>
        /// Pool object back after some time (like Destroy with time param). Uses gameobject's name as id. Specify id if you changed the object's name.
        /// </summary>
        public static void PoolObject(GameObject @object, float time)
        {
            PoolObject(@object, @object.name, time);
        }

        #endregion Pooling object

        #region Instantiating

        private static GameObject InstantiateObject(string id, bool dontDestroy = false)
        {
            var @object = GameObject.Instantiate(prefabs[id]);
            if (dontDestroy) GameObject.DontDestroyOnLoad(@object);
            @object.name = prefabs[id].name;
            return @object;
        }

        private static GameObject InstantiateObject(string id, bool activeState, bool dontDestroy = false)
        {
            var @object = InstantiateObject(id, dontDestroy);
            @object.SetActive(activeState);
            return @object;
        }

        #endregion
    }
}