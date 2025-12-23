using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

/// <summary>
/// 预先行动结束后
/// </summary>
public class BattlePreDoDesitionEndController : ControllerBase<BattlePreDoDesitionEndEventModel>
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    public override void Handle(BattlePreDoDesitionEndEventModel model)
    {
        //设置技能
        var setUnitSkillEventModel = PoolManager.GetClass<BattleSetUnitSkillEventModel>();
        setUnitSkillEventModel.SetSkillUnitList = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue()
            .Select(behaviour => BattleManager.GetUnit(behaviour.SubjectID).EntityID).ToList();
        MessageManager.DispatchMsg(setUnitSkillEventModel);
        PoolManager.RecycleClass(setUnitSkillEventModel);

        //决定后扳机
        var triggerDoDesitionMomentEventModel = PoolManager.GetClass<BattleTriggerDoDesitionMomentEventModel>();
        triggerDoDesitionMomentEventModel.DoDesitionUnitList = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue()
            .Select(behaviour => BattleManager.GetUnit(behaviour.SubjectID).EntityID).ToList();
        triggerDoDesitionMomentEventModel.IsPreDesition = true;
        MessageManager.DispatchMsg(triggerDoDesitionMomentEventModel);
        PoolManager.RecycleClass(triggerDoDesitionMomentEventModel);
        
        MessageManager.DispatchMsg<BattleOneActionWheelStartEventModel>(null);
    }
}
