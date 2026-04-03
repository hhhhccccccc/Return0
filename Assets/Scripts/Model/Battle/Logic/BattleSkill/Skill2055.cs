using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2055 : BattleSkillBase
{
    //todo 若目标体大于自身则恢复伤害量一半的体，若目标体小于自身则再造成伤害量一半的伤害
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        // TODO: ChangeHpByAttackDamage
    }

    //刚炁+70
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 70, BattleSource.Skill);
    }
}