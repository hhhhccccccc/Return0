using System;
using System.Collections.Generic;
using UnityEngine;
public class BattleUIWindow : MonoBehaviour
{
    private List<IDisposable> _events = new List<IDisposable>();

    //protected IDisposable Register<T>(Action<T> action) where T : MessageModel => GameManager.MessageManager.Register<T>(action);
  
    protected virtual void Awake()
    {
        _events = new List<IDisposable>();
        RegisterEvent();
    }

    protected virtual void RegisterEvent()
    {
        
    }

    public void Close()
    {
        OnClose();
    }

    protected virtual void OnClose()
    {
        foreach (var single in _events)
        {
            single.Dispose();
        }
        
        _events.Clear();
    }
}
