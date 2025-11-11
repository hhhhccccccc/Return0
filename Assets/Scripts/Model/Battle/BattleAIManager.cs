
using Zenject;

public class BattleAIManager : SingleModel
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    private BattleField Agent;
    private BattleField Opponent;
    public void BattleStart()
    {
        Agent = BattleManager.OtherBf;
        Opponent = BattleManager.SelfBf;
    }

    public void RoundStart()
    {
        var oppoAliveUnits = Opponent.GetAliveUnit();
        foreach (var subject in Agent.GetAliveUnit())
        {
            var oppoUnit = Util.GetRandom(oppoAliveUnits);
            BattleLogicBehaviourManager.AddOrSetBattleBehaviour(subject.EntityID, oppoUnit.EntityID,
                BattleBehaviourType.Skill, Util.GetRandomInt(1001, 1004), 0);
        }
    }

    public void RoundEnd()
    {
        
    }
}
