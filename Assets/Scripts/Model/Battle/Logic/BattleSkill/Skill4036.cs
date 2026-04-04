using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4036 : BattleSkillBase
{
    //本次行动加快2息或延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        if (Util.GetRandomBool())
        {
            DoChangeActionWheel(Subject, 2);
        }
        else
        {
            DoChangeActionWheel(Subject, -2);
        }
    }

    //敌手获得3层刚屏
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffGangPing, Subject, 3, null, BattleMomentType.BeforeClash);
    }
    
    //目标刚炁+35
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Target, BattlePropertyType.GangQi, 35, BattleSource.Skill);
    }
}