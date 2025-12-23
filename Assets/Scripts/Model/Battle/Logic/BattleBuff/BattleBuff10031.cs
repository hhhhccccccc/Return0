using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10031 : BattleBuffBase
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }

    private List<int> UnitList = new();
    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var attacker = BattleManager.GetUnit(model.SelfID);
            var attackSkillID = attacker.GetSkillID();
            //受到行动后这一息没有行动过 且当前没有正在释放的技能 且是杀式
            if (BattleUtil.SkillIsKillingStyle(attackSkillID) && !Subject.ActionWheelIsAction && Subject.GetSkill() == null && !BattleLogicBehaviourManager.BattleBehaviourRes.ContainsKey(Subject.EntityID))
            {
                if (Subject.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(GameConst.Battle.SkillCounterattack, 0), attacker))
                {
                    UnitList.Clear();
                    Subject.AddActionTimes(1);
                    BattleLogicBehaviourManager.AddOrSetBattleBehaviour(Subject.EntityID,
                        attacker.EntityID, BattleBehaviourType.Skill, GameConst.Battle.SkillCounterattack, 0);
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
}
