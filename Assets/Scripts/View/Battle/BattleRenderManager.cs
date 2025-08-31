
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public partial class BattleRenderManager
{
    private Dictionary<int, BattleUnitComponent> UnitDict = new();
    public void ResetUnitToDict(BattleUnitComponent unit) => UnitDict.Add(unit.Unit.EntityID, unit);
    public Dictionary<int, BattleUnitComponent> GetUnitDict() => UnitDict;
    public BattleUnitComponent GetUnit(int entityID) => UnitDict[entityID];
    
    [Inject] private IPoolManager PoolManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private ILogManager LogManager;
    [Inject] private IMessageManager MessageManager;
    private BattleThemeManager BattleThemeManager;
    private PlayerNodeComponent PlayerNodeComponent;
    private Camera MainCamera;
    protected override void OnAwake()
    {
        base.OnAwake();
        MainCamera = Camera.main;
        LogManager.Debug("[场景加载完毕]");
    }
    
    public void AfterBind()
    {
        var mapObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/Theme/Battle/BattleTheme.prefab");
        BattleThemeManager = mapObj.GetComponent<BattleThemeManager>();
        var playerNodeObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/Unit/Battle/BattleUnitNode.prefab");
        PlayerNodeComponent = playerNodeObj.GetComponent<PlayerNodeComponent>();
        PlayerNodeComponent.CreateBattleRole();
        LogManager.Debug("[场景人物后续加载完毕]");
    }

    protected override void OnStart()
    {
        base.OnStart();
    }
    
    protected override void RegisterEvent()
    {
        Register<MouseClickEventModel>(OnMouseClick);
        Register<RefreshBattleRenderEventModel>(OnRefreshBattleUnitRender);
    }

    private void OnMouseClick(MouseClickEventModel model)
    {
        Vector2 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        if (hit.collider != null)
        {
            var component = hit.collider.GetComponent<BattleUnitComponent>();
            if (component != null)
            { 
                component.OnClick();
            }
        }
    }

    private void OnRefreshBattleUnitRender(RefreshBattleRenderEventModel model)
    {
        PlayerNodeComponent.RefreshUnitRender(model.RefreshSelfBf, model.RefreshOtherBf);
    }
    
    public void RoundStart()
    {
        BattleThemeManager.RoundStart();
        PlayerNodeComponent.RefreshUnitRender(true, true);
    }

    public void RoundEnd()
    {
        
    }
}
