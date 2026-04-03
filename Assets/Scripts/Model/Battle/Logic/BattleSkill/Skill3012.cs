using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3012 : BattleSkillBase
{
    //施加10层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 10, null, BattleMomentType.ReleaseSkillAction);
    }

    //玄炁+20，本回合扣除1次行动次数
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 20, BattleSource.Skill);
        DoAddActionTimes(Subject, -1);
    }
    
    //造成的伤害增加50%
    public override float GetDamagePct(MomentParamModel paramModel)
    {
        return 0.5f;
    }
}