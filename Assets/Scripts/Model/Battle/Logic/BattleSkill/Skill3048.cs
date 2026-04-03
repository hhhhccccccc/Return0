using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3048 : BattleSkillBase
{
    //招式的刚炁消耗转为当前70%，至多70
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.7f, 70);
    }

    //todo 本回合受到过目标的直接伤害则威力增加35的百分比
    
    //获得3层力衰
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLiShuai, Subject, 3, null, BattleMomentType.AfterAction);
    }
}