using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1050 : BattleSkillBase
{
    public override float GetDamageReducePct()
    {
        return Config.ParamEx[0];
    }
}