using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4026 : BattleSkillBase
{
    protected override int DontBeCounter()
    {
        return 1;
    }
    
    public override bool CanIgnoreSkillDirectDamage()
    {
        if (IsInAction)
        {
            return true;
        }

        return false;
    }
}