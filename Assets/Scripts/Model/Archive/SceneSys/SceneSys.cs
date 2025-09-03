using System.Linq;
using cfg;

public class SceneSys : SingleArchiveModel
{
    /// <summary>
    /// 州（世界）
    /// </summary>
    public int WorldID;
    /// <summary>
    /// 县（地图）
    /// </summary>
    public int MapID;
    /// <summary>
    /// 地区
    /// </summary>
    public int ZoneID;
    /// <summary>
    /// 场景
    /// </summary>
    public int SceneID;
    
    public override void Init()
    {
        base.Init();
    }
}
