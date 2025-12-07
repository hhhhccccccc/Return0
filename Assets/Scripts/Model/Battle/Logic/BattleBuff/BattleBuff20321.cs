using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20321 : BattleBuffBase
{
    protected override bool OnCheckSkillCanUse(int skillGuid, BattleUnit target)
    {
        return false;
    }
}
