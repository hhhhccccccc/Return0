using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20161 : BattleBuffBase
{
    protected override bool OnCheckSkillCanUse(int skillGuid, BattleUnit target)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        return v == 0;
    }
}
