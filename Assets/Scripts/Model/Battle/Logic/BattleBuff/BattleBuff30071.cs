using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30071 : BattleBuffBase
{
    private int Round { get; set; }
    private int ActionWheel { get; set; }
    protected override void OnBuffStart()
    {
        Register<UnitTriggerReleaseSkillActionEventModel>(OnUnitTriggerReleaseSkillAction);
    }

    private void OnUnitTriggerReleaseSkillAction(UnitTriggerReleaseSkillActionEventModel model)
    {
        if (model.AttackerID == Subject.EntityID)
        {
            return;
        }
        
        if (Round == BattleLogicStateManager.Round && BattleLogicStateManager.ActionWheel - GetConfigParamInt(0) < ActionWheel)
        {
            return;
        }
        
        var attacker = BattleManager.GetUnit(model.AttackerID);
        var hit = BattleManager.GetUnit(model.HitID);
        //我方杀式命中时 且当前没有正在释放的技能 且是杀式
        if (attacker.Bf == Subject.Bf && CheckSkillIsKillingStyle(attacker, true) && !Subject.ActionWheelIsAction && Subject.GetSkill() == null && !BattleLogicBehaviourManager.BattleBehaviourRes.ContainsKey(Subject.EntityID))
        {
            if (Subject.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(GameConst.Battle.SkillFuXiaoJian, 0), hit))
            {
                var list = new List<int>();
                DoAddActionTimes(Subject, 1);
                BattleLogicBehaviourManager.AddOrSetBattleBehaviour(Subject.EntityID,
                    model.HitID, BattleBehaviourType.Skill, GameConst.Battle.SkillFuXiaoJian, 0);
                list.Add(Subject.EntityID);
                    
                var setUnitSkillEventModel = PM.GetClass<BattleSetUnitSkillEventModel>();
                setUnitSkillEventModel.SetSkillUnitList = list;
                MessageManager.DispatchMsg(setUnitSkillEventModel);
                PM.RecycleClass(setUnitSkillEventModel);
                    
                DoSetActionWheelToNow(Subject);
                DoReduceBuffLayerCount(Subject, BuffID, 1);
                Round = BattleLogicStateManager.Round;
                ActionWheel = BattleLogicStateManager.ActionWheel;
            }
        }
    }

    public override int ReduceLayerCount(int layerCount)
    {
        if (Subject.BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10067))
        {
            return LayerCount;
        }
        
        return base.ReduceLayerCount(layerCount);
    }

    protected override void OnBuffRecycle()
    {
        Round = 0;
        ActionWheel = 0;
    }
}
