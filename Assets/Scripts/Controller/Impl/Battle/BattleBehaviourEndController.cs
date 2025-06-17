using System.Collections.Generic;
using System.Linq;
using Zenject;

public class BattleBehaviourEndController : ControllerBase<BattleBehaviourEndEventModel>
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;

    private List<BattleBehaviour> BattleBehaviourList = new();
    
    public override void Handle(BattleBehaviourEndEventModel model)
    {
        var idList = model.BattleBehaviourIDList;
        foreach (var behaviourID in idList)
        {
            var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(behaviourID);
            BattleBehaviourList.Add(behaviour);
        }
    }
}
