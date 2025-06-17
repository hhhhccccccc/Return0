using Zenject;

public class BattleMomentManager : SingleModel
{
    [Inject] private IConfigManager ConfigManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager;
    [Inject] private BattleMomentEffectManager BattleMomentEffectManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    
    /// <summary>
    /// 宝具，技能，心法的扳机
    /// </summary>
    /// <param name="momentID"></param>
    /// <param name="subjectID"></param>
    public void TriggerMoment(int momentID, int subjectID)
    {
        var subject = BattleManager.GetUnit(subjectID);
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(subjectID);
        var targetID = behaviour?.TargetID ?? 0;
        var target = BattleManager.GetUnit(targetID);
        var config = ConfigManager.GetBattleMoment(momentID);
        var result = BattleMomentConditionManager.GetCondition(config.ConditionID, subject, target);
        var effectID = result ? config.SuccessMomentEffect : config.FailMomentEffect;
        BattleMomentEffectManager.OnEffect(effectID, subject, target);
    }
    
    /// <summary>
    /// 为buff添加一个施法者的扳机
    /// </summary>
    /// <param name="momentID"></param>
    /// <param name="subjectID"></param>
    /// <param name="spellcasterID"></param>
    public void TriggerMoment(int momentID, int subjectID, int spellcasterID)
    {
        var subject = BattleManager.GetUnit(subjectID);
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(subjectID);
        var targetID = behaviour?.TargetID ?? 0;
        var target = BattleManager.GetUnit(targetID);
        var spellcaster = BattleManager.GetUnit(spellcasterID);
        var config = ConfigManager.GetBattleMoment(momentID);
        var result = BattleMomentConditionManager.GetCondition(config.ConditionID, subject, target, spellcaster);
        var effectID = result ? config.SuccessMomentEffect : config.FailMomentEffect;
        BattleMomentEffectManager.OnEffect(effectID, subject, target, spellcaster);
    }
}
