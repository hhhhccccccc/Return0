using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleUseSkillPanel : Panel
{
    [AutoFind] private Transform TfSkillContent  { get; set; }
    [AutoFind] private GameObject GoSkillCurr  { get; set; }
    [AutoFind] private Image ImgSkillIconCurr  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSkillNameCurr  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSkillWelly  { get; set; }
    [AutoFind] private Transform TfLeftHeadNode  { get; set; }
    [AutoFind] private Transform TfRightHeadNode  { get; set; }
    [AutoFind] private GameObject UIBattleUnitInfoItem1  { get; set; }
    [AutoFind] private GameObject UIBattleUnitInfoItem2  { get; set; }
    [AutoFind] private Transform TfKeyContent  { get; set; }
    [AutoFind] private Image ImgKeyCount  { get; set; }
    [AutoFind] private Image ImgUpCount  { get; set; }
    [AutoFind] private Image ImgDownCount  { get; set; }
    [AutoFind] private Image ImgLeftCount  { get; set; }
    [AutoFind] private Image ImgRightCount  { get; set; }
    protected override void BindAction()
    {
    }
}
