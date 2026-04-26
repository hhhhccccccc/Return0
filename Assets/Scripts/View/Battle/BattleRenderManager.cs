
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleViewSelectData
{
    public int SelectID { get; set; }
}

public enum BattleViewState
{
    InScene = 1,
    InRole = 2,
}

public partial class BattleRenderManager
{
    private Dictionary<int, BattleUnitItem> UnitDict = new();
    public void ResetUnitToDict(BattleUnitItem unit) => UnitDict.Add(unit.Unit.EntityID, unit);
    public Dictionary<int, BattleUnitItem> GetUnitDict() => UnitDict;
    public BattleUnitItem GetUnit(int entityID) => UnitDict.TryGetValue(entityID, out var item) ? item : null;
    
    [Inject] private BattleManager BattleManager { get; set; }
    private BattleThemeManager BattleThemeManager { get; set; }
    private BattleUnitNodeItem BattleUnitNodeItem { get; set; }
    private Camera MainCamera { get; set; }
    private BattleViewState ViewState { get; set; }
    public BattleViewSelectData BattleViewSelectData = new();
    public BattleField SelfBf { get; set; }
    public BattleField OtherBf { get; set; }
    protected override void OnInstanceCreate()
    {
        MainCamera = ViewManager.MainCamera;
        SelfBf = BattleManager.SelfBf;
        OtherBf = BattleManager.OtherBf;
        LogManager.D("[场景加载完毕]");
    }
    
    public void AfterBind()
    {
        ViewState = BattleViewState.InScene;
        BattleThemeManager = CreateInstanceByType<BattleThemeManager>(ViewManager.Root);
        BattleUnitNodeItem = CreateItemByType<BattleUnitNodeItem>(ViewManager.Root);
        BattleUnitNodeItem.CreateBattleRole();
        LogManager.D("[场景人物后续加载完毕]");
    }
    
    protected override void RegisterEvent()
    {
        Register<MouseClickEventModel>(OnMouseClick);
        Register<RefreshBattleRenderEventModel>(OnRefreshBattleUnitRender);
    }

    private void OnMouseClick(MouseClickEventModel model)
    {
        if (ViewState == BattleViewState.InScene)
        {
            Vector2 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
            if (hit.collider != null)
            {
                var component = hit.collider.GetComponent<BattleUnitItem>();
                if (component != null)
                { 
                    UIManager.ShowUI<UIBattleUseSkillPanel>(ui =>
                    {
                        ui.SetSelectID(component.Unit.EntityID, true);
                    });
                }
            }
        }
    }

    private void OnRefreshBattleUnitRender(RefreshBattleRenderEventModel model)
    {
        BattleUnitNodeItem.RefreshUnitRender(model.RefreshSelfBf, model.RefreshOtherBf);
    }
    
    public void RoundStart()
    {
        BattleThemeManager.RoundStart();
        BattleUnitNodeItem.RefreshUnitRender(true, true);
    }

    public void RoundEnd()
    {
        
    }
}
