using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3105 : BattleSkillBase
{
    protected override float SkillAttackAddWelly()
    {
        return Subject.GetRandomBuffByType(BuffType.Abnormal, 0).Count * Config.SkillAttackAddWelly[0];
    }
}