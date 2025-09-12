using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class UIZonePanel : Panel
{
    [AutoFind] private Image ZoneBg  { get; set; }
    [AutoFind] private GameObject SceneBgNode  { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
    }
}
