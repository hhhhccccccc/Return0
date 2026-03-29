using UnityEngine;
using Zenject;

public class BattleEndController : ControllerBase<BattleEndEventModel>
{
    [Inject] private DiContainer DiContainer { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private UIManager UIManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private IMessageManager MessageManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    public override void Handle(BattleEndEventModel model)
    {
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            foreach (var moment in unit.BattleMomentManager.GetMoments())
            {
                moment.BattleEnd();
            }
        }
        
        LogManager.D("[战斗结束]");
        BattleManager.Clear();
        BattleLogicBehaviourManager.Clear();
        BattleLogicStateManager.Clear();
    }
}
