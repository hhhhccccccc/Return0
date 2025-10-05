
using System.Collections.Generic;
using Zenject;

public class SkillPreUseData1010 : SkillPreUseDataBase
{
    public override float GetGangQiCost()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.GangQiCost - 2 * UseCount;
    }
}
