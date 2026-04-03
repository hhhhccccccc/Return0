using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3062 : BattleSkillBase
{
    //至少造成100%力的伤害时总体减少其15%防
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var hpValue = model.GetSelfAttackHpValue(Subject.EntityID);
            var power = Subject.GetProperty(BattlePropertyType.Power);
            if (hpValue >= power)
            {
                DoChangeProperty(Target, BattlePropertyType.DefendPct, -0.15f, BattleSource.Skill);
            }
        }
    }
}