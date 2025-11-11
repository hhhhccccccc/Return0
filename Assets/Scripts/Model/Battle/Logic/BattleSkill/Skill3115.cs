using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3115 : BattleSkillBase
{
    private const int BuffID = 10041;
    protected override float SkillAddWellyRate()
    {
        var buff = Subject.GetBuff(BuffID);
        var count = buff?.LayerCount ?? 0;
        return count * Config.SkillAttackAddWelly[0];
    }
}