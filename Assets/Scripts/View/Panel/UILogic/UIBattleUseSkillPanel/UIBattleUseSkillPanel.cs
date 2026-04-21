using Zenject;

public partial class UIBattleUseSkillPanel
{
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    public void SetSelectID(int selectID)
    {
        BattleRenderManager.BattleViewSelectData.SelectID = selectID;
        var unit = BattleRenderManager.GetUnit(selectID);
        ViewManager.AdjustCameraForTwoObjects(unit.transform);
    }

    protected override void OnClose()
    {
        BattleRenderManager.BattleViewSelectData.SelectID = 0;
        ViewManager.AdjustCameraForTwoObjects();
    }
}
