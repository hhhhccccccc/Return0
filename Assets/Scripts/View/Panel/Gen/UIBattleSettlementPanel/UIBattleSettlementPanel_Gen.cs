using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleSettlementPanel : Panel
{
    [AutoFind] private GameObject Content  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
