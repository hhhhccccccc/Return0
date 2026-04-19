using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIBattleHeadItem : UIEventComponent<UIBattleHeadItem>
{
    [AutoFind] private Image ImgIcon  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
