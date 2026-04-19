using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattlePanel : Panel
{
    [AutoFind] private Button BtnStop  { get; set; }
    [AutoFind] private TextMeshProUGUI Txt  { get; set; }
    [AutoFind] private GameObject GoTopContent  { get; set; }
    [AutoFind] private Image ImgTop2  { get; set; }
    [AutoFind] private Transform TfTopLeftHeadNode  { get; set; }
    [AutoFind] private Transform TfTopRightHeadNode  { get; set; }
    [AutoFind] private Image ImgTop  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtChrono  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtWeather  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtRound  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtActionWheel  { get; set; }
    [AutoFind] private Button BtnLook  { get; set; }
    [AutoFind] private GameObject GoMiddleContent  { get; set; }
    [AutoFind] private Transform TfMiddleLeftInfoNode  { get; set; }
    [AutoFind] private Transform TfMiddleRightInfoNode  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        BtnStop.onClick.AddListener(OnBtnStop);
        BtnLook.onClick.AddListener(OnBtnLook);
    }
}
