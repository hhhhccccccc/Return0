using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleRoundStartPanel : Panel
{
    [AutoFind] private Transform TfSpine  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtChrono  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtWeather  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtRound  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
