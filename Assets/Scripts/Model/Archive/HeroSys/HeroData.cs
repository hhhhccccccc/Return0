using System.Collections.Generic;
using System.Linq;
using Zenject;

public class HeroData : IModel, IRecycle
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private ConfigHelper ConfigHelper { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    /// <summary>
    /// 唯一ID
    /// </summary>
    public int Guid { get; private set; }
    /// <summary>
    /// 英雄ID
    /// </summary>
    public int HeroID { get; private set; }
    /// <summary>
    /// 英雄名称
    /// </summary>
    public string HeroName { get; private set; }
    /// <summary>
    /// 携带的物品
    /// </summary>
    public List<GameProp> TakePropList { get; private set; } = new();
    /// <summary>
    /// 战斗属性ID
    /// </summary>
    public int HeroFightProperty { get; private set; }
    /// <summary>
    /// 携带的心法
    /// </summary>
    public List<int> CarryHeartMethod { get; private set; } = new();
    /// <summary>
    /// 穿戴的心法
    /// </summary>
    public List<int> WearHeartMethodList { get; private set; } = new();
    public void WearHeartMethod(int heartMethodID) => WearHeartMethodList.Add(heartMethodID);

    public void UnWearHeartMethod(int heartMethodID)
    {
        if (WearHeartMethodList.Contains(heartMethodID))
        {
            WearHeartMethodList.Remove(heartMethodID);
        }
    }
    public void SetHeartMethod(List<int> wearHeartMethod) => WearHeartMethodList = wearHeartMethod;
    /// <summary>
    /// 携带的武杀式
    /// </summary>
    public List<int> CarryPowerKilling { get; private set; } = new();
    /// <summary>
    /// 携带的术杀式
    /// </summary>
    public List<int> CarryArtKilling { get; private set; } = new();
    /// <summary>
    /// 携带的技御式
    /// </summary>
    public List<int> CarryTechniqueImperialStyle { get; private set; } = new();
    /// <summary>
    /// 携带的法咒式
    /// </summary>
    public List<int> CarrySpellFormula { get; private set; } = new();
    /// <summary>
    /// 额外携带的技能
    /// </summary>
    public List<int> CarryExtraSkill { get; private set; } = new();
    /// <summary>
    /// 穿戴的技能
    /// </summary>
    public List<SkillData> WearSkillList { get; private set; } = new();

    public void WearSkill(SkillData data) => WearSkillList.Add(data);
    public void UnWearSkill(SkillData data)
    {
        if (WearSkillList.Contains(data))
        {
            WearSkillList.Remove(data);
        }
    }

    public void SetWearSkill(List<int> skillList, List<int> variantIDList)
    {
        WearSkillList.Clear();
        for (int i = 0; i < skillList.Count; i++)
        {
            var model = PoolManager.GetClass<SkillData>();
            model.SkillID = skillList[i];
            model.VariantID = variantIDList[i];
            WearSkillList.Add(model);
        }
    }
    
    /// <summary>
    /// 穿戴的宝器
    /// </summary>
    public List<int> WearTreasureList { get; set; }
    public void WearTreasure(int treasureID) => WearTreasureList.Add(treasureID);
    public void UnWearTreasure(int treasureID)
    {
        if (WearTreasureList.Contains(treasureID))
        {
            WearTreasureList.Remove(treasureID);
        }
    }
    public void SetWearTreasure(List<int> treasureList) => WearTreasureList = treasureList;
    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }
    public int SlotIndex { get; set; }
    public void SetSlotIndex(int index) => SlotIndex = index;

    public void Init(int heroID, int level = 1)
    {
        Guid = System.Guid.NewGuid().GetHashCode();
        HeroID = heroID;
        var heroConfig = ConfigManager.GetHeroConfig(heroID);
        HeroName = heroConfig.HeroName;
        HeroFightProperty = heroConfig.HeroFightProperty;
        CarryHeartMethod.ClearAndAddRange(ConfigHelper.RandomCommonPool(heroConfig.HeartMethodPool).Select(data => data.ID).ToList());
        WearHeartMethodList = new List<int>();
        CarryPowerKilling.ClearAndAddRange(ConfigHelper.RandomCommonPool(heroConfig.PowerKillingPool).Select(data => data.ID).ToList());
        CarryArtKilling.ClearAndAddRange(ConfigHelper.RandomCommonPool(heroConfig.ArtKillingPool).Select(data => data.ID).ToList());
        CarryTechniqueImperialStyle.ClearAndAddRange(ConfigHelper.RandomCommonPool(heroConfig.TechniqueImperialStylePool).Select(data => data.ID).ToList());
        CarrySpellFormula.ClearAndAddRange(ConfigHelper.RandomCommonPool(heroConfig.SpellFormulaPool).Select(data => data.ID).ToList());
        CarryExtraSkill.ClearAndAddRange(ConfigHelper.RandomCommonPool(heroConfig.ExtraSkillPool).Select(data => data.ID).ToList());
        WearSkillList = new List<SkillData>();
        WearTreasureList = new List<int>();
        var gamePropPool = ConfigHelper.RandomCommonPool(heroConfig.ItemDropPool);
        TakePropList.Clear();
        foreach (var gameProp in gamePropPool)
        {
            var model = PoolManager.GetClass<GameProp>();
            model.ItemID = gameProp.ID;
            model.Count = gameProp.Num;
            TakePropList.Add(model);
        }
        Level = level;
        SlotIndex = 0;
    }

    public int GetJr()
    {
        return Level;
    }

    #region 属性
    public List<int> GetFightProperty_Variety() => ConfigHelper.GetFightProperty_Variety(HeroFightProperty);

    public float GetMaxHp()
    {
        return GetFightProperty_MaxHp();
    }
    public float GetFightProperty_MaxHp() => ConfigHelper.GetFightProperty_MaxHp(HeroFightProperty, GetJr());
    
    public float Hp { get; set; }
    public float GetFightProperty_GangQi() => ConfigHelper.GetFightProperty_GangQi(HeroFightProperty, GetJr());
    public float GetFightProperty_XuanQi() => ConfigHelper.GetFightProperty_XuanQi(HeroFightProperty, GetJr());
    public float GetFightProperty_Power() => ConfigHelper.GetFightProperty_Power(HeroFightProperty, GetJr());
    public float GetFightProperty_Tech() => ConfigHelper.GetFightProperty_Tech(HeroFightProperty, GetJr());
    public float GetFightProperty_Speed() => ConfigHelper.GetFightProperty_Speed(HeroFightProperty, GetJr());
    public float GetFightProperty_Clever() => ConfigHelper.GetFightProperty_Clever(HeroFightProperty, GetJr());
    public float GetFightProperty_Defend() => ConfigHelper.GetFightProperty_Defend(HeroFightProperty, GetJr());
    public float GetFightProperty_Break() => ConfigHelper.GetFightProperty_Break(HeroFightProperty, GetJr());
    public int GetFightProperty_KeyRecover() => ConfigHelper.GetFightProperty_KeyRecover(HeroFightProperty);
    public float GetFightProperty_GangQiRecover() => ConfigHelper.GetFightProperty_GangQiRecover(HeroFightProperty);
    public float GetFightProperty_XuanQiRecover() => ConfigHelper.GetFightProperty_XuanQiRecover(HeroFightProperty);
    public float GetFightProperty_ActionRadius() => ConfigHelper.GetFightProperty_ActionRadius(HeroFightProperty);
    public float GetFightProperty_ClashRadius() => ConfigHelper.GetFightProperty_ClashRadius(HeroFightProperty);
    public int GetFightProperty_Bgm() => ConfigHelper.GetFightProperty_Bgm(HeroFightProperty);
    #endregion
    public List<GameProp> GetTakeGameProp => TakePropList;
    public void Recycle()
    {
        foreach (var propModel in TakePropList)
        {
            PoolManager.RecycleClass(propModel);
        }
        TakePropList.Clear();
    }
}
