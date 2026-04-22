using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public abstract class ViewInstance : View
{
    protected T CreateInstanceByType<T>(Transform parent) where T : ViewInstance
    {
        var path = GetInstancePath<T>();
        var go = GetGameObject(path, parent);
        return CreateInstance<T>(go);
    }
    
    protected T CreateInstance<T>(GameObject go) where T : ViewInstance
    {
        T component = go.GetOrAddComponent<T>();
        component.UnRegisterEvent();
        component.RegisterEvent();
        if (!m_instanceChilds.Contains(component))
        {
            m_instanceChilds.Add(component);
        }
        return component;
    }
    
    private List<ViewInstance> m_instanceChilds = new();
    
    private string GetInstancePath<T>() where T : ViewInstance
    {
        return $"Assets/GameResource/Prefab/{typeof(T).Name}";
    }
    
    protected override void OnAwake()
    {
        base.OnAwake();
        RegisterEvent();
        OnInstanceCreate();
    }

    protected virtual void OnInstanceCreate()
    {
        
    }

    protected override void OnDestroy()
    {
        UnRegisterEvent();
        ReleaseItemChilds();
        ReleaseInstanceChilds();
        OnInstanceDestroy();
    }

    private void ReleaseInstanceChilds()
    {
        foreach (var instance in m_instanceChilds)
        {
            Object.Destroy(instance);
        }
        m_instanceChilds.Clear();
    }
    
    protected virtual void OnInstanceDestroy()
    {
        
    }
}