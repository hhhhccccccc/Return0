using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class BattleBuffManager : SingleModel
{
    private Dictionary<int, Type> TypeDic = new();
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    
    public BattleBuffBase AddBuff(BattleUnit target,  int buffID, BattleUnit spellCaster, int addCount, List<float> buffParam = null)
    {
        if (addCount <= 0)
        {
            return null;
        }
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
                        return null;
                    }
                }
            }
            
            return target.AddBuff(buffID, spellCaster, addCount, buffParam);;
        }

        return null;
    }

    /// <summary>
    /// 判断是否有↑类留劲buff
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool CheckTargetHasUpFirstSkillBuff(int targetID)
    {
        var target = BattleManager.GetUnit(targetID);
        if (target != null)
        {
            return GameConst.Battle.BuffUpFirstSkillList.Any(buffID => target.HasBuff(buffID));
        }

        return false;
    }
    
    /// <summary>
    /// 判断是否有↓类留劲buff
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool CheckTargetHasDownFirstSkillBuff(int targetID)
    {
        var target = BattleManager.GetUnit(targetID);
        if (target != null)
        {
            return GameConst.Battle.BuffDownFirstSkillList.Any(buffID => target.HasBuff(buffID));
        }

        return false;
    }
    
    /// <summary>
    /// 判断是否有←类留劲buff
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool CheckTargetHasLeftFirstSkillBuff(int targetID)
    {
        var target = BattleManager.GetUnit(targetID);
        if (target != null)
        {
            return GameConst.Battle.BuffLeftFirstSkillList.Any(buffID => target.HasBuff(buffID));
        }

        return false;
    }
    
    /// <summary>
    /// 判断是否有→类留劲buff
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool CheckTargetHasRightFirstSkillBuff(int targetID)
    {
        var target = BattleManager.GetUnit(targetID);
        if (target != null)
        {
            return GameConst.Battle.BuffRightFirstSkillList.Any(buffID => target.HasBuff(buffID));
        }

        return false;
    }
    
    /// <summary>
    /// 判断是否有化身类buff
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool CheckTargetHasAvatarBuff(int targetID)
    {
        var target = BattleManager.GetUnit(targetID);
        if (target != null)
        {
            return GameConst.Battle.BuffAvatarList.Any(buffID => target.HasBuff(buffID));
        }

        return false;
    }
}
