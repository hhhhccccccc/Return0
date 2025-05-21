using System;

public interface IMessageManager : IManager
{
    IDisposable Register<T>(Action<T> callback) where T : MessageModel;

    void Dispatch<T>(T msg) where T : MessageModel;

    void Clear();
}
