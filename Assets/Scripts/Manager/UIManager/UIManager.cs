using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : ManagerBase, IInitRootAfter, IUpdate
{
  private readonly Dictionary<PanelLayerType, PanelLayer> _panelLayers = new();

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

  public T GetUI<T>(string uiName) where T : Panel
  {
    var config = UIConfig.GetUIConfig(uiName);
    PanelLayerType layerType = config.LayerType;
    return this.GetLayer(layerType).GetUI<T>(uiName);
  }
  
  public Panel ShowUI(string uiName)
  {
    var config = UIConfig.GetUIConfig(uiName);
    PanelLayerType layerType = config.LayerType;
    return this.GetLayer(layerType).ShowUI(config);
  }

  public void HideUI(string uiName)
  {
    var config = UIConfig.GetUIConfig(uiName);
    PanelLayerType layerType = config.LayerType;
    this.GetLayer(layerType).HideUI(config);
  }

  public void CloseUI(string uiName)
  {
    var config = UIConfig.GetUIConfig(uiName);
    PanelLayerType layerType = config.LayerType;
    this.GetLayer(layerType).CloseUI(config);
  }
  
  public void ShowAllUI(PanelLayerType layerType)
  {
    GetLayer(layerType).ShowAllUI();
  }

  public void HideAllUI(PanelLayerType layerType)
  {
    GetLayer(layerType).ShowAllUI();
  }
  
  public void CloseAllUI(PanelLayerType layerType)
  {
    GetLayer(layerType).CloseAllUI();
  }
  public void ShowAllUI()
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.ShowAllUI();
    }
  }

  public void HideAllUI()
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.HideAllUI();
    }
  }
  
  public void CloseAllUI()
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.CloseAllUI();
    }
  }

  public void OnUpdate(float dt)
  {
    foreach (var (layerType, layer) in _panelLayers)
    {
      layer.OnUpdate(dt);
    }
  }
}
