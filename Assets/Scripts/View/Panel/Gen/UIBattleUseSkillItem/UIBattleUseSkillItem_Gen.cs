using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleUseSkillItem : Item
{
    [AutoFind] private Image ImgIcon  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtName  { get; set; }
    [AutoFind] private Transform TfKeyContent  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtCost  { get; set; }
    protected override void BindAction()
    {
    }
}
