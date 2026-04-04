using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3080 : BattleSkillBase
{
    //获得4层兽化身状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffShouHuaShen, Subject, 4, null, BattleMomentType.BeforeClash);
    }

    //施加1层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}