using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : ManagerBase, IInitRootAfter
{
  private readonly Dictionary<PanelLayerType, PanelLayer> _panelLayers = new Dictionary<PanelLayerType, PanelLayer>();

  [Inject]
  private ViewManager ViewManager { get; set; }

  [Inject]
  private DiContainer DiContainer { get; set; }

  protected override IEnumerator OnInit()
  {
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    while (!this.ViewManager.Initiated)
      yield return (object) endOfFrame;
    foreach (PanelLayerType layerType in Enum.GetValues(typeof (PanelLayerType)))
      this.GetLayer(layerType);
    yield break;
  }

  public PanelLayer GetLayer(PanelLayerType layerType)
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

  public Panel ShowUI(string uiName)
  {
    var config = UIConfig.GetUIConfig(uiName);
    PanelLayerType layerType = config.LayerType;
    return this.GetLayer(layerType).ShowPanel(config);
  }

  public void HidePanel(string uiName)
  {
    var config = UIConfig.GetUIConfig(uiName);
    PanelLayerType layerType = config.LayerType;
    this.GetLayer(layerType).HidePanel(config);
  }

  public void ClosePanel(string uiName)
  {
    var config = UIConfig.GetUIConfig(uiName);
    PanelLayerType layerType = config.LayerType;
    this.GetLayer(layerType).ClosePanel(config);
  }
  
  public void ShowAllPanel(PanelLayerType layerType)
  {
    GetLayer(layerType).ShowAllPanel();
  }

  public void HideAllPanel(PanelLayerType layerType)
  {
    GetLayer(layerType).ShowAllPanel();
  }
  
  public void CloseAllPanel(PanelLayerType layerType)
  {
    GetLayer(layerType).CloseAllPanel();
  }
  public void ShowAllPanel()
  {
    foreach (var kv in _panelLayers)
    {
      kv.Value.ShowAllPanel();
    }
  }

  public void HideAllPanel()
  {
    foreach (var kv in _panelLayers)
    {
      kv.Value.HideAllPanel();
    }
  }
  
  public void CloseAllPanel()
  {
    foreach (var kv in _panelLayers)
    {
      kv.Value.CloseAllPanel();
    }
  }
}
