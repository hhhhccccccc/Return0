using System.Collections.Generic;
using System.Linq;
using Zenject;

public class HeroData : IModel
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private ConfigHelper ConfigHelper { get; set; }
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
    /// 战斗属性ID
    /// </summary>
    public int HeroFightProperty { get; private set; }
    /// <summary>
    /// 携带的心法
    /// </summary>
    public List<int> CarryHeartMethod { get; private set; }
    /// <summary>
    /// 穿戴的心法
    /// </summary>
    public List<int> WearHeartMethodList { get; private set; }
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
    public List<int> CarryPowerKilling { get; private set; }
    /// <summary>
    /// 携带的术杀式
    /// </summary>
    public List<int> CarryArtKilling { get; private set; }
    /// <summary>
    /// 携带的技御式
    /// </summary>
    public List<int> CarryTechniqueImperialStyle { get; private set; }
    /// <summary>
    /// 携带的法咒式
    /// </summary>
    public List<int> CarrySpellFormula { get; private set; }
    /// <summary>
    /// 额外携带的技能
    /// </summary>
    public List<int> CarryExtraSkill { get; private set; }
    /// <summary>
    /// 穿戴的技能
    /// </summary>
    public List<int> WearSkillList { get; private set; }

    public void WearSkill(int skillID) => WearSkillList.Add(skillID);

    public void UnWearSkill(int skillID)
    {
        if (WearSkillList.Contains(skillID))
        {
            WearSkillList.Remove(skillID);
        }
    }
    public void SetWearSkill(List<int> skillList) => WearSkillList = skillList;
    
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
        CarryHeartMethod = ConfigHelper.RandomCommonPool(heroConfig.HeartMethodPool).Select(data => data.ID).ToList();
        WearHeartMethodList = new List<int>();
        CarryPowerKilling = ConfigHelper.RandomCommonPool(heroConfig.PowerKillingPool).Select(data => data.ID).ToList();
        CarryArtKilling = ConfigHelper.RandomCommonPool(heroConfig.ArtKillingPool).Select(data => data.ID).ToList();
        CarryTechniqueImperialStyle = ConfigHelper.RandomCommonPool(heroConfig.TechniqueImperialStylePool).Select(data => data.ID).ToList();
        CarrySpellFormula = ConfigHelper.RandomCommonPool(heroConfig.SpellFormulaPool).Select(data => data.ID).ToList();
        CarryExtraSkill = ConfigHelper.RandomCommonPool(heroConfig.ExtraSkillPool).Select(data => data.ID).ToList();
        WearSkillList = new List<int>();
        WearTreasureList = new List<int>();
        Level = level;
        SlotIndex = 0;
    }

    public int GetJr()
    {
        return Level;
    }

    public List<int> GetFightProperty_Variety() => ConfigHelper.GetFightProperty_Variety(HeroFightProperty);
    
    public float GetFightProperty_Hp() => ConfigHelper.GetFightProperty_Hp(HeroFightProperty, GetJr());
    
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
}
