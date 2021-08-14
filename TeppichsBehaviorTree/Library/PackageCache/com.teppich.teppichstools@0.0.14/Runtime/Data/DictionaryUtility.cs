using System;
using System.Collections.Generic;

namespace TeppichsTools.Data
{
    public static class DictionaryUtility
    {
        public static Dictionary<T, T1> ShallowCloneDictionary<T, T1, T2, T3>(Dictionary<T, T1> data)
            where T1 : Dictionary<T2, T3>
        {
            Dictionary<T, T1> temp = new Dictionary<T, T1>();

            
            
            return temp;
        }

        public static Dictionary<T1, T2> Clone<T1, T2>(Dictionary<T1, T2> dict)
            where T1 : ICloneable where T2 : ICloneable
        {
            if (dict == null)
                return null;

            Dictionary<T1, T2> ret = new Dictionary<T1, T2>();

            foreach (KeyValuePair<T1, T2> e in dict)
                ret[(T1) e.Key.Clone()] = (T2) e.Value.Clone();

            return ret;
        }

        /*
        public class CloneableDictionary<TKey, TValue> : Dictionary<TKey, TValue>,  ICloneable where TValue : ICloneable
        {
            public static CloneableDictionary<TKey, TValue> AndCast(CloneableDictionary<TKey, TValue> data) =>
                (CloneableDictionary<TKey, TValue>) Clone(data);
            
            public object Clone(CloneableDictionary<TKey, TValue> data)
            {
                var temp = new CloneableDictionary<TKey, TValue>();

                foreach (var VARIABLE in data)
                {
                    
                }

                return temp;
            }
        }
        */
    }
}