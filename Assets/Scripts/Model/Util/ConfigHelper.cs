
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class ConfigHelper : SingleModel
{
    [Inject] private ConfigManager ConfigManager { get; set; }

    private List<CommonPoolData> TempCommonPoolOriginList = new();
    private List<int> TempPoolWeightList = new();
    
    public List<CommonPoolData> RandomCommonPool(int poolID)
    {
        if (poolID == 0)
        {
            return new List<CommonPoolData>();
        }
        
        var config = ConfigManager.GetCommonPoolConfig(poolID);
        TempCommonPoolOriginList.Clear();
        foreach (var data in config.Pool)
        {
            TempCommonPoolOriginList.Add(data);
            TempPoolWeightList.Add(data.Weight);
        }
        
        return Util.GetRandomNoSame(TempCommonPoolOriginList, TempPoolWeightList, config.Count);
    }

    #region 获取战斗属性

    public int GetFightProperty_Variety(int propertyID)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        return config.Variety;
    }
    
    public float GetFightProperty_Hp(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.HpBase + config.HpUp * jr;
        return value.ToRound();
    }
    
    public float GetFightProperty_GangQi(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.GangQiBase + config.GangQiUp * jr;
        return value.ToRound();
    }
    
    public float GetFightProperty_XuanQi(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.XuanQiBase + config.XuanQiUp * jr;
        return value.ToRound();
    }

    public float GetFightProperty_Power(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.PowerBase + config.PowerUp * jr;
        return value.ToRound();
    }
    
    public float GetFightProperty_Tech(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.TechBase + config.TechUp * jr;
        return value.ToRound();
    }
    
    public float GetFightProperty_Speed(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.SpeedBase + config.SpeedUp * jr;
        return value.ToRound();
    }
    
    public float GetFightProperty_Clever(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.CleverBase + config.CleverUp * jr;
        return value.ToRound();
    }
    
    public float GetFightProperty_Defend(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.DefendBase + config.DefendUp * jr;
        return value.ToRound();
    }
    
    public float GetFightProperty_Break(int propertyID, int jr)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        var value = config.BreakBase + config.BreakUp * jr;
        return value.ToRound();
    }
    
    public int GetFightProperty_KeyRecover(int propertyID)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        return config.KeyRecover;
    }
    
    public float GetFightProperty_GangQiRecover(int propertyID)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        return config.GangQiRecoverNatural;
    }
    
    public float GetFightProperty_XuanQiRecover(int propertyID)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        return config.XuanQiRecoverNatural;
    }
    
    public float GetFightProperty_ActionRadius(int propertyID)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        return config.ActionRadius;
    }
    
    public float GetFightProperty_ClashRadius(int propertyID)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        return config.ClashRadius;
    }

    public int GetFightProperty_Bgm(int propertyID)
    {
        var config = ConfigManager.GetHeroFightPropertyConfig(propertyID);
        return config.BgmID;
    }
    
    #endregion
}
