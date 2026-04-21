using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class UIManager : ManagerBase, IInitRootAfter, IUpdate
{
  private readonly Dictionary<PanelLayerType, PanelLayer> _panelLayers = new();
  private readonly List<Panel> _hidePanel = new();
  private readonly List<Panel> _openPanel = new();
  private readonly Dictionary<string, Panel> _panelMap = new();
  [Inject] private IResourceManager ResourceManager { get; set; }
  [Inject] private ViewManager ViewManager { get; set; }
  [Inject] private DiContainer DiContainer { get; set; }
  
  protected override IEnumerator OnInit()
  {
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    while (!this.ViewManager.Initiated)
      yield return (object) endOfFrame;
    foreach (PanelLayerType layerType in Enum.GetValues(typeof (PanelLayerType)))
      this.GetLayer(layerType);
  }

  private PanelLayer GetLayer(PanelLayerType layerType)
  {
    PanelLayer layer1;
    if (this._panelLayers.TryGetValue(layerType, out layer1))
      return layer1;
    PanelLayer layer2 = new PanelLayer();
    this.DiContainer.Inject((object) layer2);
    layer2.Init(layerType);
    this._panelLayers.Add(layerType, layer2);
    this.DiContainer.Bind<PanelLayer>().WithId((object) layerType).FromInstance(layer2).AsTransient();
    return layer2;
  }

  public T GetUI<T>() where T : Panel
  {
    if (this._panelMap.TryGetValue(typeof(T).Name, out var panel))
      return panel as T;
    throw new Exception("Get panel error, not found panel: " + typeof (T).FullName);
  }
  
  public Panel ShowUI<T>(Action<T> action = null) where T : Panel 
  {
    Panel panel;
    var panelName = typeof(T).Name;
    if (!this._panelMap.TryGetValue(panelName, out panel))
    {
      var obj = Object.Instantiate<GameObject>(this.ResourceManager.Load<GameObject>($"Assets/GameResource/Prefab/{typeof(T).Name}"));
      if (obj.GetComponent<T>() == null)
      {
        obj.AddComponent<T>();
      }
      panel = obj.GetComponent<Panel>();
      this._openPanel.Add(panel);
      this._panelMap[panelName] = panel;
    }
      
    if (this._hidePanel.Contains(panel))
    {
      this._hidePanel.Remove(panel);
    }

    if (_openPanel.Contains(panel))
    {
      this._openPanel.Remove(panel);
    }
    
    if (!_openPanel.Contains(panel))
    {
      this._openPanel.Add(panel);
    }

    panel.name = panelName;
    var panelLayer = panel.PanelLayerType;
    Transform transform;
    (transform = panel.transform).SetParent(_panelLayers[panelLayer].Transform);
    var rectTransform = transform as RectTransform;
    // 设置锚点为拉伸模式（四边锚点分别对齐父物体四角）
    if (rectTransform != null)
    {
      rectTransform.anchorMin = Vector2.zero; // 左下角 (0,0)
      rectTransform.anchorMax = Vector2.one; // 右上角 (1,1)

      // 设置偏移量为 0
      rectTransform.offsetMin = Vector2.zero; // Left, Bottom
      rectTransform.offsetMax = Vector2.zero; // Right, Top
      
      rectTransform.localScale =Vector3.one;
    }

    panel.transform.SetAsLastSibling();
    panel.gameObject.SetActive(true);
    panel.OnShow();
    action?.Invoke(panel as T);
    return panel;
  }

  public void HideUI<T>() where T : Panel
  {
    if (!this._panelMap.TryGetValue(typeof(T).Name, out var panel))
      return;
    panel.OnHide();
    this._openPanel.Remove(panel);
    panel.gameObject.SetActive(false);
    panel.transform.SetAsFirstSibling();
    this._hidePanel.Add(panel);
  }

  public void CloseUI<T>() where T : Panel
  {
    if (!this._panelMap.TryGetValue(typeof(T).Name, out var panel))
      return;
    this._openPanel.Remove(panel);
    this._panelMap.Remove(typeof(T).Name);
    Object.Destroy((Object) panel.gameObject);
  }
  
  public void CloseUI(string uiName)
  {
    if (!this._panelMap.TryGetValue(uiName, out var panel))
      return;
    this._openPanel.Remove(panel);
    this._panelMap.Remove(uiName);
    Object.Destroy((Object) panel.gameObject);
  }

  public void OnUpdate(float dt)
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (_openPanel.Count > 0)
      {
        _openPanel[^1].Esc();
      }
    }
    
    foreach (var panel in _openPanel)
    {
      panel.ViewUpdate(dt);
    }
  }
  
  public Vector2 ConvertWorldToUIPosition(Vector3 worldPosition, RectTransform rectTransform)
  {
    // 将世界坐标转换为屏幕坐标
    Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

    // 将屏幕坐标转换为UI局部坐标
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
      rectTransform, 
      screenPoint, 
      null, // Overlay模式相机为null
      out localPoint
    );

    return localPoint;
  }
}
