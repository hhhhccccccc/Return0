using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4042 : BattleSkillBase
{
    //获得5层玄聚状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXuanJu, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    //巧+30，速+30
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.CleverInt, 30, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.SpeedInt, 30, BattleSource.Skill);
    }
}