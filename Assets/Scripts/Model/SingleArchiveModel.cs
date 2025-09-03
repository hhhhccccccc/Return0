using System;
using System.Collections;
using Zenject;

public abstract class SingleArchiveModel : ISingleArchiveModel
{
    [Inject] private IArchiveManager ArchiveManager { get; set; }
    [Inject] protected ConfigManager ConfigManager { get; set; } 
    [Inject] protected ILogManager LogManager { get; set; } 
    [Inject] private IMessageManager MessageManager { get; set; }
    [Inject] protected IPoolManager PoolManager { get; set; }
    protected void Debug(string msg) => LogManager.Debug(msg);
    protected void Error(string msg) => LogManager.Error(msg);
    protected IDisposable Register<T>(Action<T> action) where T : MessageModel => MessageManager.Register<T>(action);
    protected void Dispatch<T>(T model) where T : MessageModel => MessageManager.DispatchMsg(model);
    public virtual void Init()
    {
        
    }
    
    public void Save()
    {
        int? hashCode = this.GetType()?.FullName?.GetHashCode();
        if (!hashCode.HasValue)
            return;
        this.ArchiveManager.Save(hashCode.ToString(), (object) this);
    }
}