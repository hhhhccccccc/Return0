using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2048 : BattleSkillBase
{
    //获得3层心眼
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }

    //刚炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 10, BattleSource.Skill);
    }
}