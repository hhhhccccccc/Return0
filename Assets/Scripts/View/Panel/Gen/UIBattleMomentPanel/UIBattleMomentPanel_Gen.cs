using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleMomentPanel : Panel
{
    [AutoFind] private Transform TfSelfMomentContent  { get; set; }
    [AutoFind] private Transform TfOtherMomentContent  { get; set; }
    protected override void BindAction()
    {
    }
}
