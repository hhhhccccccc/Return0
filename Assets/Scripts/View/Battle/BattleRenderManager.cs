
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public partial class BattleRenderManager
{
    private Dictionary<int, BattleUnitComponent> UnitDict = new();
    public void ResetUnitToDict(BattleUnitComponent unit) => UnitDict.Add(unit.Unit.EntityID, unit);
    public Dictionary<int, BattleUnitComponent> GetUnitDict() => UnitDict;
    public BattleUnitComponent GetUnit(int entityID) => UnitDict[entityID];
    
    [Inject] private BattleManager BattleManager { get; set; }
    private BattleThemeManager BattleThemeManager { get; set; }
    private PlayerNodeComponent PlayerNodeComponent { get; set; }
    private Camera MainCamera { get; set; }
    protected override void OnCreate()
    {
        base.OnCreate();
        MainCamera = ViewManager.MainCamera;
        LogManager.D("[场景加载完毕]");
    }
    
    public void AfterBind()
    {
        var mapObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/Theme/Battle/BattleTheme.prefab", ViewManager.Root);
        BattleThemeManager = mapObj.GetComponent<BattleThemeManager>();
        var playerNodeObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/Unit/Battle/BattleUnitNode.prefab", ViewManager.Root);
        PlayerNodeComponent = playerNodeObj.GetComponent<PlayerNodeComponent>();
        PlayerNodeComponent.CreateBattleRole();
        LogManager.D("[场景人物后续加载完毕]");
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
