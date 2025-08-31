using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoEx
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : MonoBehaviour
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            return go.AddComponent<T>();
        }
        
        return component;
    }
}