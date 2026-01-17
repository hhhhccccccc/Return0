using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill3094 : BattleSkillBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    public override bool IsTrueDamage(DamageParamModel model)
    {
        var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
        var targetHp = target.GetProperty(BattlePropertyType.Hp);
        return targetHp <= model.GetSelfAttackTruthDamageValue(Subject.EntityID);
    }
} 