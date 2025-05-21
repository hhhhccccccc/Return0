public abstract class BattleMoment<T>
{
    protected T Model;

    /// <summary>
    /// 战斗开始时
    /// </summary>
    public abstract void BattleStart();
    /// <summary>
    /// 回合开始时
    /// </summary>
    public abstract void RoundStart();
    /// <summary>
    /// 行动决定后
    /// </summary>
    public abstract void AfterActionDecision();
    /// <summary>
    /// 收到行动时
    /// </summary>
    public abstract void UnderOtherAction();
    /// <summary>
    /// 交锋时
    /// </summary>
    public abstract void UnderConfrontation();
    /// <summary>
    /// 交锋后
    /// </summary>
    public abstract void AfterConfrontation();
    /// <summary>
    /// 命中时
    /// </summary>
    public abstract void UnderHit();
    /// <summary>
    /// 受到行动后
    /// </summary>
    public abstract void AfterOtherAction();
    /// <summary>
    /// 行动后 
    /// </summary>
    public abstract void AfterAction();
    /// <summary>
    /// 回合结束后
    /// </summary>
    public abstract void RoundEnd();
}