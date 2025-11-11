using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3020 : BattleSkillBase
{
    protected override float SkillAddWellyRate()
    {
        if (Target != null)
        {
            var targetSkill = Target.GetSkill();
            if (targetSkill != null)
            {
                var id = targetSkill.SkillID;
                var skillDamage = ConfigManager.GetBattleSkillConfig(id).Damage;
                return skillDamage * Config.SkillAttackAddWelly[0];
            }
        }

        return 0;
    }
}