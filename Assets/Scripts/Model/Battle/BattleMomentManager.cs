using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleMomentManager : SingleModel
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager;
    [Inject] private BattleMomentEffectManager BattleMomentEffectManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;

    private Queue<BattleMomentViewModel> ViewModelQueue = new();
    /// <summary>
    /// 宝具，技能，心法的扳机
    /// </summary>
    /// <param name="momentType"></param>
    /// <param name="momentID"></param>
    /// <param name="subjectID"></param>
    /// <param name="paramModel"></param>
    public Queue<BattleMomentViewModel> TriggerMoment(int momentID, int subjectID, MomentParamModel paramModel, BattleMomentType momentType)
    {
        var subject = BattleManager.GetUnit(subjectID);
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(subjectID);
        var targetID = behaviour?.TargetID ?? 0;
        var target = BattleManager.GetUnit(targetID);
        var config = ConfigManager.GetBattleMomentConfig(momentID);
        var conditionIDList = config.ConditionID;
        if (conditionIDList.Count <= 0)
        {
            foreach (var effectID in config.SuccessMomentEffect)
            {
                ViewModelQueue.Enqueue(BattleMomentEffectManager.OnEffect(effectID, subject, target, paramModel, momentType));
            }
        }
        else if (config.ConditionReleation == 1)
        {
            var result = conditionIDList.All(conditionID => BattleMomentConditionManager.GetCondition(conditionID, subject, target, paramModel));
            var effectIDList = result ? config.SuccessMomentEffect : config.FailMomentEffect;
            foreach (var effectID in effectIDList)
            {
                ViewModelQueue.Enqueue(BattleMomentEffectManager.OnEffect(effectID, subject, target, paramModel, momentType));
            }
        }
        else
        {
            var result = conditionIDList.Any(conditionID => BattleMomentConditionManager.GetCondition(conditionID, subject, target, paramModel));
            var effectIDList = result ? config.SuccessMomentEffect : config.FailMomentEffect;
            foreach (var effectID in effectIDList)
            {
                ViewModelQueue.Enqueue(BattleMomentEffectManager.OnEffect(effectID, subject, target, paramModel, momentType));
            }
        }

        return ViewModelQueue;
    }

    /// <summary>
    /// 为buff添加一个施法者的扳机
    /// </summary>
    /// <param name="momentType"></param>
    /// <param name="momentID"></param>
    /// <param name="subjectID"></param>
    /// <param name="spellCasterID"></param>
    /// <param name="paramModel"></param>
    /// <param name="layerCount">buff当前的层数</param>
    public Queue<BattleMomentViewModel> TriggerMoment(int momentID, int subjectID, int spellCasterID, MomentParamModel paramModel, int layerCount, BattleMomentType momentType)
    {
        var subject = BattleManager.GetUnit(subjectID);
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(subjectID);
        var targetID = behaviour?.TargetID ?? 0;
        var target = BattleManager.GetUnit(targetID);
        var spellCaster = BattleManager.GetUnit(spellCasterID);
        var config = ConfigManager.GetBattleMomentConfig(momentID);
        var conditionIDList = config.ConditionID;
        if (conditionIDList.Count <= 0)
        {
            foreach (var effectID in config.SuccessMomentEffect)
            {
                ViewModelQueue.Enqueue(BattleMomentEffectManager.OnEffect(effectID, subject, target, spellCaster, paramModel, layerCount, momentType));
            }
        }
        else if (config.ConditionReleation == 1)
        {
            var result = conditionIDList.All(conditionID => BattleMomentConditionManager.GetCondition(conditionID, subject, target, spellCaster, paramModel, layerCount));
            var effectIDList = result ? config.SuccessMomentEffect : config.FailMomentEffect;
            foreach (var effectID in effectIDList)
            {
                ViewModelQueue.Enqueue(BattleMomentEffectManager.OnEffect(effectID, subject, target, spellCaster, paramModel, layerCount, momentType));
            }
        }
        else
        {
            var result = conditionIDList.Any(conditionID => BattleMomentConditionManager.GetCondition(conditionID, subject, target, spellCaster, paramModel, layerCount));
            var effectIDList = result ? config.SuccessMomentEffect : config.FailMomentEffect;
            foreach (var effectID in effectIDList)
            {
                ViewModelQueue.Enqueue(BattleMomentEffectManager.OnEffect(effectID, subject, target, spellCaster, paramModel, layerCount, momentType));
            }
        }

        return ViewModelQueue;
    }
}
