using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4039 : BattleSkillBase
{
    //若未成为敌手的行动目标则获得1层匿形状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        if (CheckSelfIsOppoTarget(Subject, false))
        {
            DoAddBuff(Subject, GameConst.Battle.BuffNiXing, Subject, 1, null, BattleMomentType.DoDesitionAction);
        }
    }

    //随机获得2个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 2, ChangeKeyReason.SkillEffect);
    }

    //刚炁+25
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 25, BattleSource.Skill);
    }
}