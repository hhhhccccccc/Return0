using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleUseSkillPanel : Panel
{
    [AutoFind] private Transform TfLeftHeadNode  { get; set; }
    [AutoFind] private Transform TfRightHeadNode  { get; set; }
    protected override void BindAction()
    {
    }
}
