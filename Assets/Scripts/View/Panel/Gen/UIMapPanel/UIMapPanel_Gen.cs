using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIMapPanel : Panel
{
    [AutoFind] private Image MapBg  { get; set; }
    [AutoFind] private GameObject ZoneBgNode  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
