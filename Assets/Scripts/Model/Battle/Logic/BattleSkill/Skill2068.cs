using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2068 : BattleSkillBase
{
    //获得5层寒沁
    public override void SelfActionWheelStart()
    { 
        DoAddBuff(Target, GameConst.Battle.BuffHanXin, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
    }

    //施加3层破绽状态和3层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffPoZhan, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}