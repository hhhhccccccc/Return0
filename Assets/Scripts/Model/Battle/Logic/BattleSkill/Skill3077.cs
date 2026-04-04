using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3077 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var damage = model.GetSelfAttackHpValue(Subject.EntityID);
            var power = Subject.GetProperty(BattlePropertyType.Power);
            if (damage >= power * 1.0f)
            {
                DoChangeProperty(Subject, BattlePropertyType.GangQi, 50, BattleSource.Skill);
            }
        }
    }
}