using System;
using System.Collections.Generic;

namespace UnityGame
{
    public class DataStorage
    {
        private Dictionary<string, IData> _data = new Dictionary<string, IData>();

        public void AddOrChange<T>(T data) where T : IData
        {
            string key = typeof(T).ToString();
            _data[key] = data;
        }

        public T Get<T>() where T : IData
        {
            Type type = typeof(T);
            string key = type.ToString();

            IData data;
            if(_data.TryGetValue(key, out data))
            {
                return (T)data;
            }

            var newData = Activator.CreateInstance(type);
            return (T)newData;
        }
    }
}

