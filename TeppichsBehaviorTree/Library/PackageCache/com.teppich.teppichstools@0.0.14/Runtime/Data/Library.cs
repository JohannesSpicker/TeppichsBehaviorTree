using System;
using System.Collections.Generic;

namespace TeppichsTools.Data
{
    [Serializable]
    public class Library
    {
        public OuterDictionary outer = new OuterDictionary();

        public Library(Library data)
        {
            foreach (KeyValuePair<Type, InnerDictionary> typePair in data.outer)
            {
                outer[typePair.Key] = new InnerDictionary();

                foreach (KeyValuePair<string, object> dictPair in typePair.Value)
                    outer[typePair.Key][dictPair.Key] = dictPair.Value;
            }
        }

        public Library() { }

        public void Clear() => outer.Clear();

        public void Write<T>(string id, T value)
        {
            if (!outer.ContainsKey(typeof(T)))
                outer.Add(typeof(T), new InnerDictionary());

            outer[typeof(T)].Add(id, value);
        }

        public T Read<T>(string id)
        {
            try
            {
                return (T) outer[typeof(T)][id];
            }
            catch
            {
                return default;
            }
        }

        public void Delete<T>(string id)
        {
            try
            {
                outer[typeof(T)].Remove(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        [Serializable]
        public class OuterDictionary : UnitySerializedDictionary<Type, InnerDictionary> { }

        [Serializable]
        public class InnerDictionary : UnitySerializedDictionary<string, object> { }
    }
}