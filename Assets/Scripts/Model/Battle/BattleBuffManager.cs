using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffManager : SingleModel
{
    private Dictionary<int, Type> TypeDic = new();
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    
    public BattleBuffBase AddBuff(BattleUnit target,  int buffID, BattleUnit spellCaster, int addCount, List<float> buffParam = null, BattleMomentType momentType = BattleMomentType.None)
    {
        if (target == null || buffID <= 0 || spellCaster == null || addCount <= 0)
        {
            return null;
        }
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        if (buffConfig == null)
        {
            return null;
        }

        //药力
        if (target.HasBuffMechanism(BuffMechanism.Potion))
        {
            target.CheckPotion(buffID);
        }
    
        //心法10123 回绝 力衰和武衰
        if (target.CheckHasMethod(GameConst.Battle.HeartMethod10123) && target.HasBuff(GameConst.Battle.Buff30371) &&
            (buffID == GameConst.Battle.Buff20111 || buffID == GameConst.Battle.Buff20131))
        {
            return null;
        }
        
        //心法10124 回绝 技衰和术衰
        if (target.CheckHasMethod(GameConst.Battle.HeartMethod10124) && target.HasBuff(GameConst.Battle.Buff30381) &&
            (buffID == GameConst.Battle.Buff20121 || buffID == GameConst.Battle.Buff20141))
        {
            return null;
        }
        
        //心法10125 回绝 缓速和失衡
        if (target.CheckHasMethod(GameConst.Battle.HeartMethod10125) && target.HasBuff(GameConst.Battle.Buff30391) &&
            (buffID == GameConst.Battle.Buff20011 || buffID == GameConst.Battle.Buff20021))
        {
            return null;
        }
        
        
        //buff回绝
        //失持
        if (buffConfig.BuffType == (int)BuffType.Gain &&
            target.HasBuffMechanism(BuffMechanism.NotBeAddGainBuff))
        {
            return null;
        }
        //避殃
        if (buffConfig.BuffType == (int)BuffType.Abnormal &&
            target.HasBuffMechanism(BuffMechanism.NotBeAddAbnormalBuff))
        {
            return null;
        }
        //藏身
        if (buffConfig.BuffType == (int)BuffType.Abnormal && momentType == BattleMomentType.ReleaseSkillAction &&
            target.HasBuff(GameConst.Battle.IgnoreDebuff10121) && (target.GetSkillType() == SkillType.PowerKilling || target.GetSkillType() == SkillType.TechniqueImperialStyle))
        {
            var ignoreBuff10121 = target.GetBuff(GameConst.Battle.IgnoreDebuff10121);
            ignoreBuff10121.TriggerBuffMomentByCountIgnoreLayerCount(1, null);
            return null;
        }
        //隐魂
        if (buffConfig.BuffType == (int)BuffType.Abnormal && momentType == BattleMomentType.ReleaseSkillAction &&
            target.HasBuff(GameConst.Battle.IgnoreDebuff10131) && (target.GetSkillType() == SkillType.ArtKilling || target.GetSkillType() == SkillType.SpellFormula))
        {
            var ignoreBuff10131 = target.GetBuff(GameConst.Battle.IgnoreDebuff10131);
            ignoreBuff10131.TriggerBuffMomentByCountIgnoreLayerCount(1, null);
            return null;
        }
        
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
                    LogManager.D($"添加buff{buffID}失败, 存在{mutexID}");
                    return null;
                }
            }
        }
            
        return target.AddBuff(buffID, spellCaster, addCount, buffParam);;
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
