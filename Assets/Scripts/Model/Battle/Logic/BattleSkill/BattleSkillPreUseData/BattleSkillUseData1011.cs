
using System.Collections.Generic;
using Zenject;

public class BattleSkillUseData1011 : BattleSkillUseDataBase
{
    public override float GetXuanQiCost()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.XuanQiCost - 2 * UseCount;
    }
}
