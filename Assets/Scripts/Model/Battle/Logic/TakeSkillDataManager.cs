using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

/// <summary>
/// 携带的技能
/// </summary>
public class TakeSkillDataManager : IModel, IRecycle
{
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    /// <summary>
    /// skillGuid => BattleSkillData
    /// </summary>
    private Dictionary<int, BattleSkillData> TakeSkillDict { get; } = new();

    public void InitSkillData(List<SkillData> heroDataWearSkillList)
    {
        foreach (var data in heroDataWearSkillList)
        {
            TryAddSkillData(data.SkillID, data.VariantID);
        }
    }

    private void TryAddSkillData(int skillID, int variantID)
    {
        if (TakeSkillDict.TryGetValue(skillID, out var data))
        {
            data = PoolManager.GetClass<BattleSkillData>();
            data.Guid = Util.CombSkillGuid(skillID, variantID);
            data.SkillID = skillID;
            data.VariantID = variantID;
            TakeSkillDict.Add(skillID, data);
        }
    }

    public BattleSkillData GetSkillDataBySkillID(int skillID, int variantID)
    {
        foreach (var kv in TakeSkillDict)
        {
            var data = kv.Value;
            if (data.SkillID == skillID && data.VariantID == variantID)
            {
                return data;
            }
        }

        return null;
    }

    public BattleSkillData GetSkillDataByGuidID(int guid) => TakeSkillDict.TryGetValue(guid, out var data) ? data : null;
    
    public void Recycle()
    {
        foreach (var kv in TakeSkillDict)
        {
            PoolManager.RecycleClass(kv.Value);   
        }
        
        TakeSkillDict.Clear();
    }
}
