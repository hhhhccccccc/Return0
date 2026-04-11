using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3031 : BattleSkillBase
{
    //招式的刚炁消耗转为当前80%，至多80
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.8f, 80);
    }

    //获得4层力衰
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLiShuai, Subject, 4, null, BattleMomentType.AfterAction);
    }
}