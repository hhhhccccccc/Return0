using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30071 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    private List<int> UnitList = new();
    private int Round { get; set; }
    private int ActionWheel { get; set; }
    protected override void OnStart()
    {
        base.OnStart();
        Register<UnitTriggerReleaseSkillActionEventModel>(OnUnitTriggerReleaseSkillAction);
    }

    private void OnUnitTriggerReleaseSkillAction(UnitTriggerReleaseSkillActionEventModel model)
    {
        if (model.AttackerID == Subject.EntityID)
        {
            return;
        }
        
        if (Round == BattleLogicStateManager.Round && BattleLogicStateManager.ActionWheel - Config.ParamEx[0].ToInt() < ActionWheel)
        {
            return;
        }
        
        var attacker = BattleManager.GetUnit(model.AttackerID);
        var attackSkillID = attacker.GetSkillID();
        var hit = BattleManager.GetUnit(model.HitID);
        //我方杀式命中时 且当前没有正在释放的技能 且是杀式
        if (attacker.Bf == Subject.Bf && BattleUtil.SkillIsKillingStyle(attackSkillID) && !Subject.ActionWheelIsAction && Subject.GetSkill() == null && !BattleLogicBehaviourManager.BattleBehaviourRes.ContainsKey(Subject.EntityID))
        {
            if (Subject.CheckSkillCanDoDesition_Logic(Util.CombSkillGuid(GameConst.Battle.SkillFuXiaoJian, 0), hit))
            {
                UnitList.Clear();
                Subject.AddActionTimes(1);
                BattleLogicBehaviourManager.AddOrSetBattleBehaviour(Subject.EntityID,
                    model.HitID, BattleBehaviourType.Skill, GameConst.Battle.SkillFuXiaoJian, 0);
                UnitList.Add(Subject.EntityID);
                    
                var setUnitSkillEventModel = PoolManager.GetClass<BattleSetUnitSkillEventModel>();
                setUnitSkillEventModel.SetSkillUnitList = UnitList;
                MessageManager.DispatchMsg(setUnitSkillEventModel);
                PoolManager.RecycleClass(setUnitSkillEventModel);
                    
                Subject.SetActionWheelToNow();
                BattleLogicStateManager.CallAddUnitToNowLogicCalculate(Subject.EntityID);
                ReduceLayerCount(1);
                Round = BattleLogicStateManager.Round;
                ActionWheel = BattleLogicStateManager.ActionWheel;
            }
        }
    }

    public override void Recycle()
    {
        Round = 0;
        ActionWheel = 0;
        base.Recycle();
    }
}
