using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using Zenject;

public class PoolManager : IManager
{
    public IEnumerator Init()
    {
        _gameObjectPool = new Dictionary<string, Queue<GameObject>>();
        this.BattlePoolRoot = new GameObject("[BattleRoot]").transform;
        BattlePoolRoot.gameObject.SetActive(false);
        
        _classPool = new Dictionary<Type, Queue<object>>();
        this.Initiated = true;
        yield break;
    }
    [Inject] private ResourceManager ResourceManager { get; set; }

    private Dictionary<string, Queue<GameObject>> _gameObjectPool;
    private Transform BattlePoolRoot { get; set; }
    public bool Initiated { get; set; }

    public GameObject GetGameObject(string path, Action<GameObject> callback)
    {
        Queue<GameObject> source;
        GameObject prefab;
        if (!this._gameObjectPool.TryGetValue(path, out source) || !source.Any())
        {
            prefab = UnityEngine.Object.Instantiate<GameObject>(
                    this.ResourceManager.Load<GameObject>(path));
        }
        else
        {
            prefab = source.Dequeue();
        }
        
        callback(prefab);
        
        return prefab;
    }

    public void ReleaseGameObject(string path, GameObject gameObject)
    {
        gameObject.transform.position = new Vector3(1000, 1000);
        Queue<GameObject> source;
        if (!this._gameObjectPool.TryGetValue(path,out source))
        {
           source=new Queue<GameObject>();
           this._gameObjectPool.Add(path,source);
        }
        source.Enqueue(gameObject);
    }
    
    private Dictionary<Type, Queue<object>> _classPool; 

    [Inject] private DiContainer DiContainer;
    
    /// <summary>
    /// 从对象池获取对象（如果没有则创建新对象）
    /// </summary>
    public T GetClass<T>() where T : class, new()
    {
        var type = typeof(T);
        if (!_classPool.TryGetValue(type, out var queue))
        {
            queue = new Queue<object>();
            _classPool[type] = queue;
        }

        if (queue.Count > 0)
        {
            return (T)queue.Dequeue();
        }
        
        return DiContainer.Resolve<T>();
    }

    /// <summary>
    /// 将对象回收到对象池
    /// </summary>
    public void RecycleClass<T>(T obj) where T : class
    {
        if (obj == null) return;

        var type = typeof(T);
        if (!_classPool.TryGetValue(type, out var queue))
        {
            queue = new Queue<object>();
            _classPool[type] = queue;
        }

        queue.Enqueue(obj);
    }

    /// <summary>
    /// 清空所有对象池
    /// </summary>
    public void ClearGameObjectPool()
    {
        foreach (var queue in _classPool.Values)
        {
            queue.Clear();
        }
        _classPool.Clear();
    }

    public void ClearClassPool()
    {
        _gameObjectPool.Clear();
    }
}