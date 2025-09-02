using System.Collections;
using Zenject;

public abstract class SingleArchiveModel : ISingleArchiveModel
{
    [Inject] private IArchiveManager ArchiveManager { get; set; }
    [Inject] protected ConfigManager ConfigManager { get; set; } 
    [Inject] protected ILogManager LogManager { get; set; } 
    public bool IsInit { get; set; }

    public void Init()
    {
        if (!IsInit)
        {
            IsInit = true;
            InitPlayerData();
        }
        
        InitGameData();
    }
    
    /// <summary>
    /// 初始化第一次上游戏数据
    /// </summary>
    protected abstract void InitPlayerData();

    /// <summary>
    /// 初始化后面上游戏数据
    /// </summary>
    protected virtual void InitGameData()
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