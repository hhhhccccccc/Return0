using System.Collections.Generic;
using System.Linq;

public partial class UIBattleTeamInfoItem
{
    private BattleField Bf { get; set; }
    private List<UIBattleTeamUnitInfoItem> UnitInfoList = new();
    public void SetBf(BattleField selfBf)
    {
        Bf = selfBf;
        InitUnitInfo();
    }

    private void InitUnitInfo()
    {
        var units = Bf.GetBattleUnitDict().Values.ToList();
        CreateItems(UnitInfoList, units.Count, TfContent);
        for (int i = 0; i < UnitInfoList.Count; i++)
        {
            UnitInfoList[i].Init(units[i]);
        }
    }

    public void RefreshSkillBehaviour()
    {
        foreach (var info in UnitInfoList)
        {
            info.RefreshSkillBehaviour();
        }
    }

    protected override void OnRelease()
    {
        UnitInfoList.Clear();
    }
}
