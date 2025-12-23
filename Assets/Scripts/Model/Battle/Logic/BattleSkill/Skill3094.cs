using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill3094 : BattleSkillBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    public override bool IsTrueDamage(DamageParamModel model)
    {
        var target = BattleManager.GetUnit(model.OtherID);
        var targetHp = target.GetProperty(BattlePropertyType.Hp);
        return targetHp <= model.GetSelfTruthDamageValue(Subject.EntityID);
    }
} 