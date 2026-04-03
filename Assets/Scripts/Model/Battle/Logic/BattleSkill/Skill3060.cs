using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3060 : BattleSkillBase
{
    //玄炁+10
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }

    //施加1层伤口状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
       DoAddBuff(Target, GameConst.Battle.BuffShangKou, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //todo 触发3次目标的伤口效果 TriggerBuffTimes
    public override void AfterAction(MomentParamModel paramModel)
    {
        
    }
}