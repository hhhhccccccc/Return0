using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2009 : BattleSkillBase
{
    protected override float SkillAttackAddWelly()
    {
        return Config.SkillAttackAddWelly[0] * Subject.GetKeyCount();
    }
}