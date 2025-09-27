using System.Linq;
using Zenject;

public class BattleMomentCondition_CheckSelfIsOppoTarget : BattleMomentCondition
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    protected override bool OnCondition()
    {
        var self = GetUnitByParamID(1);
        if (self != null)
        {
            var check = Config.ParamList[0].ToInt() == 1;
            if (check)
            {
                var opponentList = BattleLogicBehaviourManager.GetOpponentBehaviourByEntityID(self.EntityID);
                return opponentList.Any(behaviour => behaviour.TargetID == self.EntityID);
            }
            else
            {
                var opponentList = BattleLogicBehaviourManager.GetOpponentBehaviourByEntityID(self.EntityID);
                return opponentList.Any(behaviour => behaviour.TargetID != self.EntityID);
            }
        }

        return false;
    }
}