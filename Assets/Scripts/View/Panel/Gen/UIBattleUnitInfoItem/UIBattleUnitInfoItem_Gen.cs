using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleUnitInfoItem : Item
{
    [AutoFind] private Image ImgXuanQi  { get; set; }
    [AutoFind] private Image ImgIcon  { get; set; }
    [AutoFind] private Image ImgGangQi  { get; set; }
    [AutoFind] private Image ImgHp  { get; set; }
    [AutoFind] private Transform TfBuffContent  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtGangQi  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtXuanQi  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtHp  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtKeyCount  { get; set; }
    protected override void BindAction()
    {
    }
}
