using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

public class PanelLayer
{
  private readonly List<Panel> _hidePanel = new();
  private readonly List<Panel> _openPanel = new();
  private readonly Dictionary<string, Panel> _panelMap = new();

  [Inject]
  private IResourceManager ResourceManager { get; set; }

  [Inject]
  private ViewManager ViewManager { get; set; }
  [Inject]
  private DiContainer DiContainer { get; set; }

  private Canvas Canvas { get; set; }

  public void Init(PanelLayerType layerType)
  {
    GameObject gameObject = new GameObject($"[{layerType} Layer]");
    gameObject.transform.SetParent(this.ViewManager.UIRoot);
    this.Canvas = gameObject.AddComponent<Canvas>();
    this.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    Canvas canvas = this.Canvas;
    int num;
    switch (layerType)
    {
      case PanelLayerType.Background:
        num = 0;
        break;
      case PanelLayerType.Midground:
        num = 10;
        break;
      case PanelLayerType.Foreground:
        num = 20;
        break;
      case PanelLayerType.Top:
        num = 30;
        break;
      case PanelLayerType.Pop:
        num = 40;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (layerType), (object) layerType, (string) null);
    }
    canvas.sortingOrder = num;
    this.Canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
    CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    canvasScaler.referenceResolution = new Vector2((float) GameConst.ReferenceResolutionX, (float) GameConst.ReferenceResolutionY);
    canvasScaler.matchWidthOrHeight = GameConst.MatchWidthOrHeight;
    gameObject.AddComponent<GraphicRaycaster>();
  }

  public Panel ShowUI(SingleUIConfig config)
  {
      Panel panel;
      if (!this._panelMap.TryGetValue(config.UIName, out panel))
      {
        panel = Object.Instantiate<GameObject>(this.ResourceManager.Load<GameObject>(config.PrefabPath), this.Canvas.transform).GetComponent<Panel>();
        panel.UIInfo = config;
        this._openPanel.Add(panel);
        this._panelMap[config.UIName] = panel;
      }
      if (this._hidePanel.Contains(panel))
      {
        this._hidePanel.Remove(panel);
        this._openPanel.Add(panel);
      }
      panel.transform.SetAsLastSibling();
      panel.gameObject.SetActive(true);
      panel.EntrustDisposablesClear();
      panel.OnShow();
      return panel;
  }

  public T GetPanel<T>(string uiName) where T : Panel
  {
    if (this._panelMap.TryGetValue(uiName, out var panel))
      return panel as T;
    throw new Exception("Get panel error, not found panel: " + typeof (T).FullName);
  }

  public void HideUI(SingleUIConfig config)
  {
    if (!this._panelMap.TryGetValue(config.UIName, out var panel))
      return;
    panel.EntrustDisposablesClear();
    panel.OnHide();
    this._openPanel.Remove(panel);
    panel.gameObject.SetActive(false);
    panel.transform.SetAsFirstSibling();
    this._hidePanel.Add(panel);
  }
  
  public void CloseUI(SingleUIConfig config)
  {
    if (!this._panelMap.TryGetValue(config.UIName, out var panel))
      return;
    this._openPanel.Remove(panel);
    this._panelMap.Remove(config.UIName);
    Object.Destroy((Object) panel.gameObject);
  }

  public void CloseAllUI()
  {
    foreach (Panel panel in this._openPanel)
      this.CloseUI(panel.UIInfo);
  }

  public void HideAllUI()
  {
    foreach (Panel panel in this._openPanel)
      this.HideUI(panel.UIInfo);
  }

  public void ShowAllUI()
  {
    foreach (Panel panel in this._hidePanel.ToList<Panel>())
      this.ShowUI(panel.UIInfo);
  }
}
