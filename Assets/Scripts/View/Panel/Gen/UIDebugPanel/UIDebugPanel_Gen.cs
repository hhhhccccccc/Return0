using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIDebugPanel : Panel
{
    [AutoFind] private TMP_InputField InputSelfID  { get; set; }
    [AutoFind] private TMP_InputField InputOtherID  { get; set; }
    [AutoFind] private Button BtnConfirm  { get; set; }
    protected override void BindAction()
    {
        BtnConfirm.onClick.AddListener(OnBtnConfirm);
    }
}
