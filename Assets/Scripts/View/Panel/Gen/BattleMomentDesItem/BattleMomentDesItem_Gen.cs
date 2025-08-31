using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class BattleMomentDesItem : UIComponent
{
    [AutoFind] private TextMeshProUGUI Txt  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
