using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleMomentPanel : Panel
{
    [AutoFind] private GameObject SelfMomentContent  { get; set; }
    [AutoFind] private GameObject OtherMomentContent  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
