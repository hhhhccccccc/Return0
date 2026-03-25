using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattlePanel : Panel
{
    [AutoFind] private Transform TfRightMenu  { get; set; }
    [AutoFind] private Button BtnCancel  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtCancel  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSubject  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSkillID  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtTarget  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtState  { get; set; }
    [AutoFind] private Button BtnStart  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtStart  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSkillCost  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        BtnCancel.onClick.AddListener(OnBtnCancel);
        BtnStart.onClick.AddListener(OnBtnStart);
    }
}
