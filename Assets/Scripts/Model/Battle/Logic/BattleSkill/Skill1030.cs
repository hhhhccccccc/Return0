using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1030 : BattleSkillBase
{
    //todo 在水域场景可使用
    
    //若未成为敌手的行动目标则获得1层匿形状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        if (CheckSelfIsOppoTarget(false))
        {
            DoAddBuff(Subject, GameConst.Battle.BuffNiXing, Subject, 1, null, BattleMomentType.DoDesitionAction);
        }
    }
    //刚炁+20
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 20, BattleSource.Skill);
    }
}