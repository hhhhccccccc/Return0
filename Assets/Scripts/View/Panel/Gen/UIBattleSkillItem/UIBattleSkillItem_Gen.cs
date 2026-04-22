using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class BattleSkillItem : EventItem<BattleSkillItem>
{
    [AutoFind] private TextMeshProUGUI TxtName  { get; set; }
    protected override void BindAction()
    {
    }
}
