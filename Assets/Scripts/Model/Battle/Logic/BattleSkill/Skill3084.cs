using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3084 : BattleSkillBase
{
    protected override float SkillAddWellyRate()
    {
        return Subject.GetRandomBuffByType(BuffType.Abnormal, 0).Count * Config.SkillAddWellyRate[0];
    }
}