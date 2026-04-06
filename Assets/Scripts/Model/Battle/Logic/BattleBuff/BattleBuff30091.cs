using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30091 : BattleBuffBase
{
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
        if (CheckSkillIsKillingStyle(attacker, true) && !Subject.ActionWheelIsAction && Subject.GetSkill() == null && !BattleLogicBehaviourManager.BattleBehaviourRes.ContainsKey(Subject.EntityID))
        {
            if (Subject.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(GameConst.Battle.SkillFuXiaoJian, 0), attacker))
            {
                var list = new List<int>();
                Subject.AddActionTimes(1);
                BattleLogicBehaviourManager.AddOrSetBattleBehaviour(Subject.EntityID,
                    model.AttackerID, BattleBehaviourType.Skill, GameConst.Battle.SkillFuXiaoJian, 0);
                list.Add(Subject.EntityID);
                    
                var setUnitSkillEventModel = PM.GetClass<BattleSetUnitSkillEventModel>();
                setUnitSkillEventModel.SetSkillUnitList = list;
                MessageManager.DispatchMsg(setUnitSkillEventModel);
                PM.RecycleClass(setUnitSkillEventModel);
                    
                DoSetActionWheelToNow(Subject);
                DoReduceBuffLayerCount(Subject, BuffID, 1);
            }
        }
    }
}
