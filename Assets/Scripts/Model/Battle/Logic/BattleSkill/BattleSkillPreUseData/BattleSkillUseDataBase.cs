
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleSkillUseDataBase : IModel
{
    [Inject] protected ConfigManager ConfigManager { get; set; }
    public int SkillID;
    public int UseCount;
    public Stack<LastUseSkillState> LastUseSkillStateStack { get; set; }
    public LastUseSkillState GetLastUseSkillState()
    {
        if (LastUseSkillStateStack.Any())
        {
            return LastUseSkillStateStack.Peek();
        }

        return LastUseSkillState.None;
    }

    public virtual int GetSkillID()
    {
        return SkillID;
    }
    
    public virtual float GetDamage()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.Damage;
    }

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

    public virtual SkillType GetSkillType()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return (SkillType)skillConfig.SkillType;
    }
    
    public virtual DamageType GetDamageType()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return (DamageType)skillConfig.DamageType;
    }
}


public enum LastUseSkillState
{
    None = 0,
    UseSuccess = 1,
    BeCounter = 2,
}