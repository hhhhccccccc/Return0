using System.Collections.Generic;
using cfg;

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
    /// 计算息
    /// </summary>
    public void CalculateActionWheel();
    /// <summary>
    /// 决定行动的调用
    /// </summary>
    public void DoDesitionAction();
    /// <summary>
    /// 息开始的调用
    /// </summary>
    public void ActionWheelStart();
    /// <summary>
    /// 行动前
    /// </summary>
    public void BeforeAction();
    /// <summary>
    /// 受到行动前调用
    /// </summary>
    public void BeforeUnderAction();
    /// <summary>
    /// 交锋前
    /// </summary>
    public void BeforeClash(MomentParamModel paramModel);
    /// <summary>
    /// 交锋后
    /// </summary>
    public void AfterClash(MomentParamModel paramModel);
    /// <summary>
    /// 技能释放成功
    /// </summary>
    public void ReleaseSkillAction(MomentParamModel paramModel);
    /// <summary>
    /// 受到行动后调用
    /// </summary>
    public void AfterUnderAction(MomentParamModel paramModel);
    /// <summary>
    /// 行动后 
    /// </summary>
    public void AfterAction(MomentParamModel paramModel);
    /// <summary>
    /// 回合结束后
    /// </summary>
    public void RoundEnd();

    public void EnqueueViewModel(BattleMomentType momentType, Queue<BattleMomentViewModel> viewModelQueue);
}
