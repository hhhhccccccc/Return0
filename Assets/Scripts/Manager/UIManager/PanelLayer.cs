using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

public class PanelLayer
{
  [Inject] private ViewManager ViewManager { get; set; }
  [Inject] private DiContainer DiContainer { get; set; }
  private Canvas Canvas { get; set; }
  public Transform Transform { get; set; }

  private const int SortingLayerBase = 0;
  private const int SortingLayerDelta = 50;

  public void Init(PanelLayerType layerType)
  {
    GameObject gameObject = new GameObject($"[{layerType} Layer]");
    gameObject.transform.SetParent(this.ViewManager.UIRoot);
    this.Canvas = gameObject.AddComponent<Canvas>();
    this.Transform = gameObject.transform;
    gameObject.AddComponent<GraphicRaycaster>();
    //this.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    Canvas canvas = this.Canvas;
    canvas.sortingOrder = (int)layerType * 100;
    this.Canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localPosition = Vector3.zero;
    var rectTransform = (RectTransform)gameObject.transform;
    rectTransform.anchorMin = new Vector2(0, 0);  // 左下角锚点
    rectTransform.anchorMax = new Vector2(1, 1);  // 右上角锚点
    rectTransform.offsetMin = Vector2.zero;  // 左、下边距 = 0
    rectTransform.offsetMax = Vector2.zero;  // 右、上边距 = 0
    gameObject.layer = 5;
  }
}
