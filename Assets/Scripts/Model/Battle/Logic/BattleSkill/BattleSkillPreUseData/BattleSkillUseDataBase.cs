
using System.Collections.Generic;
using Zenject;

public class BattleSkillUseDataBase : IModel
{
    [Inject] protected ConfigManager ConfigManager { get; set; }
    public int SkillID;
    public int UseCount;

    public virtual float GetGangQiCost()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.GangQiCost;
    }
    
    public virtual float GetXuanQiCost()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.XuanQiCost;
    }
    
    public virtual List<int> GetKeyCost()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.NeedKey;
    }
}
