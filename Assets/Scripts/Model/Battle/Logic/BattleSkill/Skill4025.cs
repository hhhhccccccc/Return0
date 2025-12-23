using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4025 : BattleSkillBase
{
    public override bool CanIgnoreSkillDirectDamage()
    {
        if (IsInAction)
        {
            return true;
        }

        return false;
    }
}