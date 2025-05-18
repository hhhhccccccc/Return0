using System;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : IManager
{
    private Dictionary<Type, MessageTracker<MessageModel>> _msgs;
    
    public IEnumerator Init()
    {
        _msgs = new();
        yield break;
    }

    public IDisposable Register<T>(Action<T> callback) where T : MessageModel
    {
        if (!this._msgs.TryGetValue(typeof(T), out var msgType))
        {
            msgType = new MessageTracker<MessageModel>();
            this._msgs.Add(typeof(T), msgType);
        }

        MessageReporter<MessageModel> report = new MessageReporter<MessageModel>();
        return report.Subscribe(msgType, o => callback((T)o));
    }

    public void Dispatch<T>(T msg) where T : MessageModel
    {
        if (this._msgs.TryGetValue(typeof(T), out var msgType))
        {
            msgType.OnNext(msg);
        }
    }

    public void Clear()
    {
        _msgs.Clear();
    }
   
}
