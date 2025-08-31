using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class BattleSettlementItem : UIComponent
{
    [AutoFind] private GameObject MomentContent  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
