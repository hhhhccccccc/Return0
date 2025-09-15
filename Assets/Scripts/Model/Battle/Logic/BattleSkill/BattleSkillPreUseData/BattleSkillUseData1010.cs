
using System.Collections.Generic;
using Zenject;

public class BattleSkillUseData1010 : BattleSkillUseDataBase
{
    public override float GetGangQiCost()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.GangQiCost - 2 * UseCount;
    }
}
