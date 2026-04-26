using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UITipPanel : Panel
{
    [AutoFind] private TextMeshProUGUI TxtTip  { get; set; }
    protected override void BindAction()
    {
    }
}
