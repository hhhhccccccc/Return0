using Zenject;

public class BattleRoundStartController : ControllerBase<BattleRoundStartEventModel>
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleDataManager BattleDataManager{ get; set; }
    [Inject] private BattleRenderManager BattleRenderManager{ get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager{ get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager{ get; set; }
    [Inject] private BattleRecordManager BattleRecordManager{ get; set; }
    [Inject] private BattleAIManager BattleAIManager{ get; set; }
    [Inject] private InputManager InputManager{ get; set; }
    public override void Handle(BattleRoundStartEventModel model)
    {
        BattleManager.RoundStart();//回合开始
        BattleLogicBehaviourManager.RoundStart();
        BattleLogicStateManager.RoundStart();
        BattleRenderManager.RoundStart();
        BattleAIManager.RoundStart();
        InputManager.SetBattleInputValid(true);

        var panel = UIManager.GetUI<UIBattlePanel>();
        panel.SetTopActive(false);
        UIManager.ShowUI<UIBattleRoundStartPanel>(ui =>
        {
            ui.Play();
        });
    }
}