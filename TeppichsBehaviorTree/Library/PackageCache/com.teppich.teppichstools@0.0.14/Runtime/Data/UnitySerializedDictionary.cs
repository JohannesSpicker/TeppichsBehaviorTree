using System.Collections.Generic;
using UnityEngine;

namespace TeppichsTools.Data
{
    /// <summary>
    ///     Exploits Unity's serialization protocol to support dictionary serialization.
    ///     Make concrete to implement as Unity doesn't serialize generic types.
    ///     Code from: https://odininspector.com/tutorials/serialize-anything/serializing-dictionaries
    /// </summary>
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
                                                                    ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private readonly List<TKey> keyData = new List<TKey>();

        [SerializeField, HideInInspector]
        private readonly List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < keyData.Count && i < valueData.Count; i++)
                this[keyData[i]] = valueData[i];
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            keyData.Clear();
            valueData.Clear();

            foreach (KeyValuePair<TKey, TValue> item in this)
            {
                keyData.Add(item.Key);
                valueData.Add(item.Value);
            }
        }
    }
}