using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20151 : BattleBuffBase
{
   
    protected override bool OnCheckSkillCanUse(int skillGuid, BattleUnit target)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        var config = ConfigManager.GetBattleSkillConfig(s);
        return config.NeedKey.Count != GetConfigParamInt(0);
    }
}
