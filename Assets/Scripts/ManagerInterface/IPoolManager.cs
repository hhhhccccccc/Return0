using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolManager : IManager
{
     GameObject GetGameObject(string path, Action<GameObject> callback = null);
     void ReleaseGameObject(string path, GameObject gameObject);
     T GetClass<T>() where T : class, new();
     void RecycleClass<T>(T obj) where T : class;
     void ClearGameObjectPool();
     void ClearClassPool();
}
