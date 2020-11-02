using System;
using System.Collections.Generic;

namespace Kuhpik
{
    [Serializable]
    public sealed class TypeRepository<TValue> where TValue : class
    {
        Dictionary<Type, TValue> dataBase;

        public T Get<T>() where T : class
        {
            return dataBase[typeof(T)] as T;
        }

        public void TryCreateData<T>(T data) where T : TValue
        {
            if (!dataBase.ContainsKey(typeof(T)))
            {
                dataBase.Add(typeof(T), data);
            }
        }

        public void OverrideData<T>(T data) where T : TValue
        {
            if (dataBase.ContainsKey(typeof(T)))
            {
                dataBase[typeof(T)] = data;
            }
        }

        public void Remove<T>(T data) where T : TValue
        {
            if (dataBase.ContainsKey(typeof(T)))
            {
                dataBase.Remove(typeof(T));
            }
        }

        public bool HasType<T>() where T : TValue
        {
            return dataBase.ContainsKey(typeof(T));
        }

        public TypeRepository(Dictionary<Type, TValue> dataBase)
        {
            this.dataBase = dataBase;
        }

        public TypeRepository()
        {
            dataBase = new Dictionary<Type, TValue>();
        }
    }
}
