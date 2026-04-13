using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2054 : BattleSkillBase
{
    /// <summary>
    /// 施加3层赤沸状态
    /// </summary>
    /// <param name="paramModel"></param>
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffChiFei, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    //消耗目标25玄炁
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Target, BattlePropertyType.XuanQi, -25f, BattleSource.Skill);
    }
}