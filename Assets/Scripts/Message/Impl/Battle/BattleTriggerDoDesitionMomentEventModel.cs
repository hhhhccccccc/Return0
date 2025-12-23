using System.Collections.Generic;

/// <summary>
/// 战斗一轮息的扳机计算
/// </summary>
public class BattleTriggerDoDesitionMomentEventModel : MessageModel
{
    /// <summary>
    /// 触发决定行动扳机的角色
    /// </summary>
    public List<int> DoDesitionUnitList { get; set; }
    /// <summary>
    /// 是否是预先行动
    /// </summary>
    public bool IsPreDesition { get; set; }
}
