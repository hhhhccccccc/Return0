using System.Collections.Generic;
using cfg;

public partial class UIBattleUnitInfoItem
{
    private List<BattleBuffBase> BuffList = new();
    private List<UIBattleBuffItem> BuffItemList = new();
    public void Refresh(BattleUnit unit)
    {
        var maxHp = unit.GetProperty(BattlePropertyType.MaxHp);
        var hp = unit.GetProperty(BattlePropertyType.Hp);
        var maxGangQi = unit.GetProperty(BattlePropertyType.MaxGangQi);
        var gangQi = unit.GetProperty(BattlePropertyType.GangQi);
        var maxXuanQi = unit.GetProperty(BattlePropertyType.MaxXuanQi);
        var xuanQi = unit.GetProperty(BattlePropertyType.XuanQi);
        var maxKeyCount = unit.GetKeyPropertyMax();
        var keyCount = unit.GetAllKeyCount();
        
        TxtHp.SetText($"{hp.ToInt()}/{maxHp.ToInt()}");
        TxtGangQi.SetText($"{gangQi.ToInt()}/{maxGangQi.ToInt()}");
        TxtXuanQi.SetText($"{xuanQi.ToInt()}/{maxXuanQi.ToInt()}");
        TxtKeyCount.SetText($"{keyCount}/{maxKeyCount}");

        ImgHp.fillAmount = hp / maxHp;
        ImgGangQi.fillAmount = gangQi / maxGangQi;
        ImgXuanQi.fillAmount = xuanQi / maxXuanQi;

        BuffList.ClearAndAddRange(unit.GetBuffList());
        CreateItems(BuffItemList, BuffList.Count, TfBuffContent);
        for (int i = 0; i < BuffItemList.Count; i++)
        {
            BuffItemList[i].Refresh(BuffList[i]);
        }
    }

    protected override void OnRelease()
    {
        BuffItemList.Clear();
    }
}
