using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2047 : BattleSkillBase
{
    //获得30%技的护体状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var tech = Subject.GetProperty(BattlePropertyType.Tech);
        DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (int)(tech * 0.3f), null, BattleMomentType.ReleaseSkillAction);
    }
}