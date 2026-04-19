using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleTeamInfoItem : UIComponent
{
    [AutoFind] private Transform TfContent  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
