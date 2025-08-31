using System;
using System.Collections.Generic;
using Zenject;

public class BattleBuffManager : SingleModel
{
    private Dictionary<int, Type> TypeDic = new();
    
    [Inject] private ConfigManager ConfigManager;

    [Inject] private ILogManager LogManager;
    
    [Inject] private IPoolManager PoolManager;

    public Type GetBuffType(int buffID)
    {
        if (!TypeDic.TryGetValue(buffID, out var type))
        {
            var config = ConfigManager.GetBattleBuffConfig(buffID);
            if (config != null)
            {
                type = Type.GetType(config.Script);
            }
            TypeDic.Add(buffID, type);
        }

        return type;
    }
    
    public bool AddBuff(BattleUnit target,  int buffID, BattleUnit spellCaster, int addCount, List<float> buffParam = null)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        if (buffConfig != null)
        {
            var relationConfig = ConfigManager.GetBattleBuffRelationConfig(buffID);
            if (relationConfig != null)
            {
                foreach (var disposeID in relationConfig.DisposeBuff)
                {
                    target.ClearBuff(disposeID);
                }
            
                foreach (var mutexID in relationConfig.MutexBuff)
                {
                    if (target.GetBuff(mutexID) != null)
                    {
                        LogManager.Debug($"添加buff{buffID}失败, 存在{mutexID}");
                        return false;
                    }
                }
            }
            
            target.AddBuff(buffID, spellCaster, addCount, buffParam);
            return true;
        }

        return false;
    }
}
