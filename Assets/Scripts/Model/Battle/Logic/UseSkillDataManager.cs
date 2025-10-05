using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class UseSkillDataManager : IModel, IRecycle
{
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    private List<SkillUseDataBase> UseSkillDataList = new();

    public void AddUseSkillData(int skillID, int round, int endActionWheel)
    {
        var data = PoolManager.GetClass<SkillUseDataBase>();
        data.SkillID = skillID;
        data.Round = round;
        data.EndActionWheel = endActionWheel;
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
    
    public void Recycle()
    {
        foreach (var data in UseSkillDataList)
        {
            PoolManager.RecycleClass(data);
        }
        
        UseSkillDataList.Clear();
    }
}
