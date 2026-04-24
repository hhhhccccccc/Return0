using System.Collections.Generic;
using cfg;

public partial class UIBattleUseSkillItem
{
    private BattleUnit Unit { get; set; }
    private BattleSkillData SkillData { get; set; }
    private List<UIBattleKeyTxtItem> m_txtItemList = new();
    private List<BattleKeyType> m_costKeyTypeList = new();
    public void Refresh(BattleUnit unit, BattleSkillData data)
    {
        Unit = unit;
        SkillData = data;
        var skillID = data.SkillID;
        var config = ConfigManager.GetBattleSkillConfig(skillID);
        SetSprite(ImgIcon, config.Icon);
        TxtName.SetText(config.Name);
        var keyCost = unit.PreUseSkillDataManager.GetSkillPreUseKeyCost(data.Guid);
        m_costKeyTypeList.Clear();
        for (int i = 0; i < keyCost.Count; i++)
        {
            m_costKeyTypeList.Add((BattleKeyType)keyCost[i]);
        }
        CreateItems(m_txtItemList, m_costKeyTypeList.Count, TfKeyContent);
        for (int i = 0; i < m_costKeyTypeList.Count; i++)
        {
            m_txtItemList[i].Refresh(m_costKeyTypeList[i]);
        }
    }

    public BattleSkillData CheckSameKey(List<BattleKeyType> checkList)
    {
        if (m_costKeyTypeList.Count != checkList.Count)
        {
            return null;
        }
        for (int i = 0; i < checkList.Count; i++)
        {
            if (checkList[i] != m_costKeyTypeList[i])
            {
                return null;
            }
        }

        return SkillData;
    }
}
