using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3070 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffFanJi, Subject, 1, null, BattleMomentType.BeforeClash);
    }

    //玄炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}