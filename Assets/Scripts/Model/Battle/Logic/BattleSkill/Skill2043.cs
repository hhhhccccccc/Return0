using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2043 : BattleSkillBase
{
    //招式的玄炁消耗转为当前70%，至多70，获得2层缓速
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.7f, 70);
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 2, null, BattleMomentType.DoDesitionAction);
    }

    //施加2层刚屏
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffGangPing, Subject, 2, null, BattleMomentType.BeforeClash);
    }

    //施加2层刚屏
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffGangPing, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //刚炁+当前30%（至少9）
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.GangQi, 0.3f, 9, BattleSource.Skill);
    }

    //造成的伤害增加50%
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        return 0.5f;
    }
}