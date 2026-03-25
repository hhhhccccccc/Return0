
using Zenject;

public class BattleAIManager : SingleModel
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    private BattleField Agent { get; set; }
    private BattleField Opponent { get; set; }
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
            var skillID = subject.TakeSkillDataManager.GetTakeSkillData()[0].SkillID;
            BattleLogicBehaviourManager.AddOrSetBattleBehaviour(subject.EntityID, oppoUnit.EntityID,
                BattleBehaviourType.Skill, skillID, 0);
        }
    }

    public void RoundEnd()
    {
        
    }
}
