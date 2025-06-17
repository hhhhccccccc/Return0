using System.Collections.Generic;
using System.Linq;
using Zenject;

public class BattleOneActionWheelLogicCalculateController : ControllerBase<BattleOneActionWheelLogicCalculateEventModel>
{
    [Inject] private IPoolManager PoolManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    
    private List<BattleUnit> InActionUnits;//初始行动的角色
    private List<BattleBehaviour> battleBehaviours;//初始行动的角色
    private List<BattleUnit> OutActionUnits;//行动完的角色
    public override void Handle(BattleOneActionWheelLogicCalculateEventModel model)
    {
        BattleLogicStateManager.SetBattleState(BattleState.ActionWheelLogicCalculate);
        InActionUnits = model.ActionWheelUnit.Select(entityID => BattleManager.GetUnit(entityID)).ToList();//当前息行动的角色
        battleBehaviours = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue()
            .Where(behaviour => InActionUnits.Any(unit => unit.EntityID == behaviour.SubjectID)).ToList();//行动角色的指令
        OutActionUnits = new List<BattleUnit>();
        var unitBeChooseKillingSkill = new List<int>();
        while (InActionUnits.Count > 0)
        { 
            /*息溢值最高且未被选为杀式目标的角色的行动优先演出
             *其次是息溢值最高的角色
             *再者是未被选为杀式目标的角色
             最后是被选为杀式目标的角色*/
            
            foreach (var behaviour in battleBehaviours)
            {
                var tempSkillType = BattleUtil.GetSkillTypeBySkillID(behaviour.SkillID);
                if (BattleUtil.SkillIsKillingStyle(tempSkillType))
                {
                    unitBeChooseKillingSkill.Add(behaviour.TargetID);
                }
            }
            var sortList = InActionUnits.OrderByDescending(unit => unit.ActionWheelOut).
                ThenByDescending(unit => unitBeChooseKillingSkill.Any(id => unit.EntityID == id) ? -1 : 1);
            
            var firstActionUnit = sortList.First();
            var subjectBehaviour = battleBehaviours.First(behaviour => behaviour.SubjectID == firstActionUnit.EntityID);
            var skillID = subjectBehaviour.SkillID;
            var battleClashCalculateModel = PoolManager.GetClass<BattleClashCalculateModel>();
            battleClashCalculateModel.SubjectActionModel = new BattleClashActionModel
            {
                SubjectID = subjectBehaviour.SubjectID,
                TargetID = subjectBehaviour.TargetID,
                SkillID = subjectBehaviour.SkillID
            };
            var skillType = BattleUtil.GetSkillTypeBySkillID(skillID);
            if (!BattleUtil.SkillIsKillingStyle(skillType) ||
                InActionUnits.All(u => u.EntityID != subjectBehaviour.TargetID)) //单方面行动
            {
                battleClashCalculateModel.ClashType = BattleClashType.SingleAction;
            }
            else if (BattleUtil.SkillIsKillingStyle(skillType))
            {
                if (InActionUnits.Any(u => u.EntityID == subjectBehaviour.TargetID))
                {
                    var targetBehaviour =
                        battleBehaviours.First(behaviour => behaviour.SubjectID == subjectBehaviour.TargetID);
                    battleClashCalculateModel.TargetActionModel = new BattleClashActionModel
                    {
                        SubjectID = targetBehaviour.SubjectID,
                        TargetID = targetBehaviour.TargetID,
                        SkillID = targetBehaviour.SkillID
                    };
                    if (targetBehaviour.TargetID != firstActionUnit.EntityID)
                    {
                        battleClashCalculateModel.ClashType = BattleClashType.SingleClash;
                    }
                    else
                    {
                        battleClashCalculateModel.ClashType = BattleClashType.DoubleClash;
                    }
                }
            }

            //对该行动进行计算
            var clashType = battleClashCalculateModel.ClashType;
            var subjectAction = battleClashCalculateModel.SubjectActionModel;
            var targetAction = battleClashCalculateModel.TargetActionModel;
            var subject = BattleManager.GetUnit(subjectAction.SubjectID);
            if (!subject.GetBeCounter())
            {
                //todo 战斗跳过回合  战斗资源不足以支付行动后的消耗或行动目标丢失 中断行动 不产生资源消耗
                if (false)
                {
                    subject.ReduceActionTimes();
                    continue;
                }
                
                var subjectReleaseSkill = true;
                if (clashType == BattleClashType.SingleAction)
                {
                    if (subjectReleaseSkill)
                    {
                        //技能释放成功后的扳机
                        
                        //计算逻辑

                        //行动后的扳机
                        TriggerAfterActionMoment(subject);
                    }
                    
                    
                    UnitActionEnd(subject);
                }
                else if (clashType == BattleClashType.SingleClash)
                {
                    //释放成功
                    if (subjectReleaseSkill)
                    {
                        //技能释放成功后的扳机
                        
                        //交锋前的扳机
                        TriggerBeforeClashMoment(subject);

                        //计算逻辑

                        //交锋后的扳机
                        TriggerAfterClashMoment(subject);
                        
                        //行动后的扳机
                        TriggerAfterActionMoment(subject);
                    }
                    UnitActionEnd(subject);
                }
                else if (clashType == BattleClashType.DoubleClash)
                {
                    var target = BattleManager.GetUnit(targetAction.SubjectID);
                    var targetReleaseSkill = true;
                    //释放成功
                    if (subjectReleaseSkill)
                    {
                        //交锋前的扳机
                        TriggerBeforeClashMoment(subject);
                    }

                    if (targetReleaseSkill)
                    {
                        //交锋前的扳机
                        TriggerBeforeClashMoment(target);
                    }

                    //计算逻辑


                    if (subjectReleaseSkill)
                    {
                        //交锋后的扳机
                        TriggerAfterClashMoment(subject);
                        TriggerAfterActionMoment(subject);
                    }

                    if (targetReleaseSkill)
                    {
                        //交锋后的扳机
                        TriggerAfterClashMoment(target);
                        TriggerAfterActionMoment(target);
                    }

                    UnitActionEnd(subject);
                    UnitActionEnd(target);
                }
            }
            
            unitBeChooseKillingSkill.Clear();
        }
        

        foreach (var actionUnit in OutActionUnits)
        {
            actionUnit.OneActionWheelEnd();
        }
        
        //这一轮息计算结束
        BattleLogicStateManager.TryEnd();
    }

    private void UnitActionEnd(BattleUnit unit)
    {
        if (InActionUnits.Contains(unit))
        {
            InActionUnits.Remove(unit);
        }

        if (!OutActionUnits.Contains(unit))
        {
            OutActionUnits.Add(unit);
        }

        var behaviour = battleBehaviours.FirstOrDefault(behaviour => behaviour.SubjectID == unit.EntityID);
        if (behaviour != null)
        {
            battleBehaviours.Remove(behaviour);
        }
        
        unit.ActionEnd();
    }

    private void TriggerReleaseSkillActionMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.ReleaseSkillAction();
        }
    }
    
    private void TriggerBeforeClashMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.BeforeClash();
        }
    }

    private void TriggerUnderHitMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.UnderHit();
        }
    }
    
    private void TriggerAfterClashMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.AfterClash();
        }
    }
    
    private void TriggerAfterActionMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.AfterAction();
        }
    }
}
