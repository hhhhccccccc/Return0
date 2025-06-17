using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class  UIBattlePanel : Panel
{
    [AutoFind] private GameObject RightMenu  { get; set; }
    [AutoFind] private Button BtnSkill1  { get; set; }
    [AutoFind] private Button BtnSkill2  { get; set; }
    [AutoFind] private Button BtnSkill3  { get; set; }
    [AutoFind] private Button BtnSkill4  { get; set; }
    [AutoFind] private Button BtnCancel  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSubject  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtSkillID  { get; set; }
    [AutoFind] private TextMeshProUGUI TxtTarget  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        BtnSkill1.onClick.AddListener(OnBtnSkill1);
        BtnSkill2.onClick.AddListener(OnBtnSkill2);
        BtnSkill3.onClick.AddListener(OnBtnSkill3);
        BtnSkill4.onClick.AddListener(OnBtnSkill4);
        BtnCancel.onClick.AddListener(OnBtnCancel);
    }
}
