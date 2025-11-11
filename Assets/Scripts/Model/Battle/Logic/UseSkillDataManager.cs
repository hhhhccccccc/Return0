using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class UseSkillDataManager : IModel, IRecycle
{
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    /// <summary>
    /// id => Guid
    /// </summary>
    private List<SkillUseDataBase> UseSkillDataList = new();

    public void AddUseSkillData(int skillGuid, int round, int endActionWheel, List<bool> clashStateList)
    {
        var data = PoolManager.GetClass<SkillUseDataBase>();
        data.Guid = skillGuid;
        (data.SkillID, data.VariantID) = Util.UnCombSkillGuid(skillGuid);
        data.Round = round;
        data.EndActionWheel = endActionWheel;
        data.ClashStateList.AddRange(clashStateList);
        UseSkillDataList.Add(data);
    }

    /// <summary>
    /// 本回合是否使用过武杀式
    /// </summary>
    /// <returns></returns>
    public bool CheckNowRoundUsedPowerKilling()
    {
        return UseSkillDataList.Any(data =>
            data.Round == BattleLogicStateManager.Round &&
            BattleUtil.GetSkillTypeBySkillID(data.SkillID) == SkillType.PowerKilling);
    }
    
    /// <summary>
    /// 本回合是否使用过术杀式
    /// </summary>
    /// <returns></returns>
    public bool CheckNowRoundUsedArtKilling()
    {
        return UseSkillDataList.Any(data =>
            data.Round == BattleLogicStateManager.Round &&
            BattleUtil.GetSkillTypeBySkillID(data.SkillID) == SkillType.ArtKilling);
    }

    public bool CheckSkillLastClashState(int skillID, bool isSuccess)
    {
        var data = UseSkillDataList.LastOrDefault(skillData => skillData.SkillID == skillID);
        if (data != null)
        {
            return data.ClashStateList.Contains(isSuccess);
        }

        return false;
    }
    
    public void Recycle()
    {
        foreach (var data in UseSkillDataList)
        {
            PoolManager.RecycleClass(data);
        }
        
        UseSkillDataList.Clear();
    }
}
