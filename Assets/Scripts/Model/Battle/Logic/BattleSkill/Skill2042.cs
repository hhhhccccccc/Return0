using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2042 : BattleSkillBase
{
    //todo 50%概率对自身造成30%技的伤害
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3600001 - DamageToTargetByProperty
        // TODO: DamageToTargetByProperty
    }

    //刚炁+25
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 25, BattleSource.Skill);
    }
}