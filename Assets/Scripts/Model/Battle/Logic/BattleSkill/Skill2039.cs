using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2039 : BattleSkillBase
{
    //招式的玄炁消耗转为当前40%，行动加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.4f, 0);
        DoChangeActionWheel(Subject, 1);
    }

    //对目标造成160%技的伤害，施加5层玄屏状态，若目标为鬼怪且消耗其全部刚炁
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        if (Target.CheckVariety(HeroVariety.Ghost))
        {
            DoSetProperty(Target, BattlePropertyType.GangQi, 0, BattleSource.Skill);
        }
    }
    //todo 若目标为鬼怪则造成的伤害加倍且消耗其全部刚炁

    
    //随机获得2个键
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 2, ChangeKeyReason.SkillEffect);
    }
}