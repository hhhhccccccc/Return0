using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleKeyTxtItem : Item
{
    [AutoFind] private TextMeshProUGUI TxtKey  { get; set; }
    protected override void BindAction()
    {
    }
}
