using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleSkillItem : UIEventComponent<UIBattleSkillItem>
{
    [AutoFind] private TextMeshProUGUI TxtName  { get; set; }
    protected override void BindAction()
    {
    }
}
