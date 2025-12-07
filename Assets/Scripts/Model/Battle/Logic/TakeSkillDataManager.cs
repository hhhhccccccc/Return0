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
    private DictAndList<int, BattleSkillData> TakeSkillDict { get; } = new();

    public List<BattleSkillData> GetTakeSkillData() => TakeSkillDict.GetListValue();
    
    public void InitSkillData(List<SkillData> heroDataWearSkillList)
    {
        foreach (var data in heroDataWearSkillList)
        {
            TryAddSkillData(data.SkillID, data.VariantID);
        }
    }

    private void TryAddSkillData(int skillID, int variantID)
    {
        var data = TakeSkillDict.TryGetValue(skillID);
        if (data == null)
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
        foreach (var data in TakeSkillDict.GetListValue())
        {
            if (data.SkillID == skillID && data.VariantID == variantID)
            {
                return data;
            }
        }

        return null;
    }

    public BattleSkillData GetSkillDataByGuidID(int guid) => TakeSkillDict.TryGetValue(guid);
    
    public void Recycle()
    {
        foreach (var data in TakeSkillDict.GetListValue())
        {
            PoolManager.RecycleClass(data);   
        }
        
        TakeSkillDict.Clear();
    }
}
