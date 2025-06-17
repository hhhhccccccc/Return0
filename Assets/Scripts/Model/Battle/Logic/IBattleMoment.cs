public interface IBattleMoment
{
    /// <summary>
    /// 战斗开始时
    /// </summary>
    public void BattleStart();
    /// <summary>
    /// 回合开始时
    /// </summary>
    public void RoundStart();
    /// <summary>
    /// 计算息的时候调用
    /// </summary>
    public void CalculateActionWheel();
    /// <summary>
    /// 决定行动的调用
    /// </summary>
    public void DoDesitionAction();
    /// <summary>
    /// 这一息开始的时候调用
    /// </summary>
    public void StartActionWheel();
    /// <summary>
    /// 被作为目标的时候调用
    /// </summary>
    public void AsTargetAction(bool fromIsTeam, int skillID);
    /// <summary>
    /// 技能释放成功
    /// </summary>
    public void ReleaseSkillAction();
    /// <summary>
    /// 交锋前
    /// </summary>
    public void BeforeClash();
    /// <summary>
    /// 命中时
    /// </summary>
    public void UnderHit();
    /// <summary>
    /// 交锋后
    /// </summary>
    public void AfterClash();
    /// <summary>
    /// 行动后 
    /// </summary>
    public void AfterAction();
    /// <summary>
    /// 回合结束后
    /// </summary>
    public void RoundEnd();
}
