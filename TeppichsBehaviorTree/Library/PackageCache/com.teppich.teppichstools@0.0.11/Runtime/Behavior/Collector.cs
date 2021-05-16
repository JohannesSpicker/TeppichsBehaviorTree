using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Collector<T> : MonoBehaviour where T : MonoBehaviour
{
    public static List<T> collection = new List<T>();

    private T thing;

    private void OnEnable() => collection.Add(thing ??= GetComponent<T>());

    private void OnDisable() => collection.Remove(thing);
}