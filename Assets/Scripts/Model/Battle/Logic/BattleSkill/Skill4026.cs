using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4026 : BattleSkillBase
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