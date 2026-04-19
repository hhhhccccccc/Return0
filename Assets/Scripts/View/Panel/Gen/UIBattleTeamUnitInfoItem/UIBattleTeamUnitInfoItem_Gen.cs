using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleTeamUnitInfoItem : UIComponent
{
    [AutoFind] private Transform TfHeadNode  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtName  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSkill  { get; set; }
    [AutoFind] private Image ImgSkill  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtActionWheel  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
