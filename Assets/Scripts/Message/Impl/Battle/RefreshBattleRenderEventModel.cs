public class RefreshBattleRenderEventModel : MessageModel
{
    /// <summary>
    /// 战斗状态
    /// </summary>
    public BattleState BattleState;
    /// <summary>
    /// 是否是刷新自己整个战场
    /// </summary>
    public bool RefreshSelfBf;
    /// <summary>
    /// 是否是刷新对手整个战场
    /// </summary>
    public bool RefreshOtherBf;
    /// <summary>
    /// 是否刷新UIBattle
    /// </summary>
    public bool RefreshUIBattle;
}
