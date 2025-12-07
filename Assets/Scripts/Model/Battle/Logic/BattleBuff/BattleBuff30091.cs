using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30091 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    private List<int> UnitList = new();
    protected override void OnBuffStart()
    {
        base.OnBuffStart();
        Register<UnitTriggerBeforeUnderActionMomentEventModel>(OnUnitTriggerBeforeUnderActionMoment);
    }

    private void OnUnitTriggerBeforeUnderActionMoment(UnitTriggerBeforeUnderActionMomentEventModel model)
    {
        if (EffectTarget == null || !EffectTarget.IsAlive())
        {
            return;
        }

        if (model.HitID != EffectTarget.EntityID)
        {
            return;
        }

        if (model.ClashType != BattleClashType.SingleClash)
        {
            return;
        }
        
        var attacker = BattleManager.GetUnit(model.AttackerID);
        var attackSkillID = attacker.GetSkillID();
        if (BattleUtil.SkillIsKillingStyle(attackSkillID) && !Subject.ActionWheelIsAction && Subject.GetSkill() == null && !BattleLogicBehaviourManager.BattleBehaviourRes.ContainsKey(Subject.EntityID))
        {
            if (Subject.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(GameConst.Battle.SkillFuXiaoJian, 0), attacker))
            {
                UnitList.Clear();
                Subject.AddActionTimes(1);
                BattleLogicBehaviourManager.AddOrSetBattleBehaviour(Subject.EntityID,
                    model.AttackerID, BattleBehaviourType.Skill, GameConst.Battle.SkillFuXiaoJian, 0);
                UnitList.Add(Subject.EntityID);
                    
                var setUnitSkillEventModel = PoolManager.GetClass<BattleSetUnitSkillEventModel>();
                setUnitSkillEventModel.SetSkillUnitList = UnitList;
                MessageManager.DispatchMsg(setUnitSkillEventModel);
                PoolManager.RecycleClass(setUnitSkillEventModel);
                    
                Subject.SetActionWheelToNow();
                BattleLogicStateManager.CallAddUnitToNowLogicCalculate(Subject.EntityID);
                ReduceLayerCount(1);
            }
        }
    }
}
