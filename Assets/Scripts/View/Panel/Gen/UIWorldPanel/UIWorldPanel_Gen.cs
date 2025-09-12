using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIWorldPanel : Panel
{
    [AutoFind] private Image WorldBg  { get; set; }
    [AutoFind] private GameObject MapBgNode  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
