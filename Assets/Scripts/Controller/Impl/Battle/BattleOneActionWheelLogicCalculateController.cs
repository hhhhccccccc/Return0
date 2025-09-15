using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleOneActionWheelLogicCalculateController : ControllerBase<BattleOneActionWheelLogicCalculateEventModel>
{
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleDataManager BattleDataManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private BattleRecordManager BattleRecordManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    
    private List<BattleUnit> InActionUnits = new();//初始行动的角色
    private List<BattleBehaviour> battleBehaviours = new();//初始行动的角色
    private List<BattleUnit> OutActionUnits = new();//行动完的角色
    
    private BattleRecordModel CurrentRecordModel;
    public override void Handle(BattleOneActionWheelLogicCalculateEventModel model)
    {
        BattleLogicStateManager.SetAfterStartActionWheel(true);
        BattleLogicStateManager.SetBattleState(BattleState.ActionWheelLogicCalculate);
        BattleLogicStateManager.RegisterAddUnitToNowLogicCalculate(UnitAddAction);
        foreach (var entityID in model.ActionWheelUnit)
        {
            UnitAddAction(entityID);
        }
        OutActionUnits.Clear();
        var unitBeChooseKillingSkill = new List<int>();
        while (InActionUnits.Count > 0)
        { 
            /*息溢值最高且未被选为杀式目标的角色的行动优先演出
             *其次是息溢值最高的角色
             *再者是未被选为杀式目标的角色
             最后是被选为杀式目标的角色*/
            foreach (var behaviour in battleBehaviours)
            {
                if (BattleManager.GetUnit(behaviour.SubjectID).SkillIsKillingStyle())
                {
                    unitBeChooseKillingSkill.Add(behaviour.TargetID);
                }
            }
            var sortList = InActionUnits.OrderByDescending(unit => unit.ActionWheelOut).
                ThenByDescending(unit => unitBeChooseKillingSkill.Any(id => unit.EntityID == id) ? -1 : 1);
            var subject = sortList.First();
            
            var subjectBehaviour = battleBehaviours.First(behaviour => behaviour.SubjectID == subject.EntityID);
            var target = BattleManager.GetUnit(subjectBehaviour.TargetID);
            
            //移除下次行动前的效果
            RemoveBeforeNextActionEffect(subject);
            //如果不满足招式释放条件(气)则直接跳过行动
            if (!subject.CheckReleaseSkillEnough())
            {
                UnitEndAction(subject);
                BeforeActionJumpByResource(subject, target);
                continue;
            }
            
            //招式是否被打没掉
            if (subject.GetBeCounter())
            {
                UnitEndAction(subject);
                BeforeActionJumpByBeCounter(subject, target);
                continue;
            }
            
            TriggerBeforeActionMoment(subject);
            TriggerBeforeUnderActionMoment(target);
            
            var clashType = BattleClashType.None;
            var skillIsKillingStyle = subject.SkillIsKillingStyle(); 
            //如果技能是非杀招 或者 为杀式但受击者在本息不存在行动 或者 为杀式但受击者本息已行动 或者 为杀式但受击者没有资源参与交锋 （则为单方面行动）
            if (!skillIsKillingStyle || InActionUnits.All(u => u.EntityID != subjectBehaviour.TargetID) || !target.CheckReleaseSkillEnough()) //单方面行动
            {
                clashType = BattleClashType.SingleAction;
            }
            //如果是杀招 且 B在本息行动但还未行动 且不互相为目标 为单方面交锋 否则 为双向交锋
            else if (InActionUnits.Any(u => u.EntityID == subjectBehaviour.TargetID))
            {
                var targetBehaviour = battleBehaviours.First(behaviour => behaviour.SubjectID == subjectBehaviour.TargetID);
                clashType = (targetBehaviour.TargetID == subject.EntityID && target.SkillIsKillingStyle()) ? BattleClashType.DoubleClash : BattleClashType.SingleClash;
            }
            
            var subjectParamModel = PoolManager.GetClass<DamageParamModel>();
            var targetParamModel = PoolManager.GetClass<DamageParamModel>();
            subjectParamModel.BattleClashType = clashType;
            targetParamModel.BattleClashType = clashType;

            //如果是双向交锋 对方移除下次行动前的效果
            if (clashType == BattleClashType.DoubleClash)
            {
                RemoveBeforeNextActionEffect(target);
            }
            
            if (clashType == BattleClashType.SingleAction)
            {
                CurrentRecordModel = PoolManager.GetClass<SingleActionRecordModel>();
            }
            else if (clashType == BattleClashType.SingleClash)
            {
                CurrentRecordModel = PoolManager.GetClass<SingleClashRecordModel>();
            }
            else
            {
                CurrentRecordModel = PoolManager.GetClass<DoubleClashRecordModel>();
            }

            CurrentRecordModel.SubjectID = subject.EntityID;
            CurrentRecordModel.TargetID = target.EntityID;
            //添加行动前的扳机效果  
            BattleRecordManager.SetCurrentAndCacheRecordModel(CurrentRecordModel);
            CurrentRecordModel.CheckSubjectCostPullFight = true;
            if (!subject.CheckReleaseSkillEnough())
            {
                CurrentRecordModel.CheckSubjectCostGenerateAction = false;
                CostSkillNeedResource(subject);
                TriggerAfterUnderActionMoment(target, targetParamModel);
                TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                UnitEndAction(subject);
                AddBattleRecordModel(CurrentRecordModel);
                continue;
            }
            
            CurrentRecordModel.CheckSubjectCostGenerateAction = true;
            
            if (clashType == BattleClashType.SingleAction)
            {
                Debug($"{subject.EntityID} : 单方面行动 : {target.EntityID}");
                CostSkillNeedResource(subject);
                CalculateSkillDamageLogic(subject, target, ref subjectParamModel, ref targetParamModel);
                TriggerReleaseSkillActionMoment(subject, subjectParamModel);
                TriggerAfterUnderActionMoment(target, targetParamModel);
                TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                UnitEndAction(subject);
            }
            else if (clashType == BattleClashType.SingleClash)
            {
                Debug($"{subject.EntityID} : 单向交锋 : {target.EntityID}");
                var clashModel = CurrentRecordModel as SingleClashRecordModel;
                TriggerBeforeClashMoment(subject, subjectParamModel);
                TriggerBeforeClashMoment(target, targetParamModel);
                var subjectReleaseSkill = subject.CheckReleaseSkillEnough();
                var targetReleaseSkill = target.CheckReleaseSkillEnough();

                clashModel.CheckSubjectCostInClash = subjectReleaseSkill;
                clashModel.CheckTargetCostInClash = targetReleaseSkill;
                
                if (subjectReleaseSkill && targetReleaseSkill)
                {
                    var subjectDamageRate = subject.GetSkillDamageRateFight();
                    var targetDamageRate = target.GetSkillDamageRateFight();
                    
                    clashModel.SetInClashSkillDamageRate(subject.EntityID, subjectDamageRate);
                    clashModel.SetInClashSkillDamageRate(target.EntityID, targetDamageRate);
                    
                    TriggerAfterClashMoment(subject, subjectParamModel);
                    TriggerAfterClashMoment(target, targetParamModel);
                    
                    if (Math.Abs(subjectDamageRate - targetDamageRate) <= 0.001f)//威力相同
                    {
                        CostSkillNeedResource(subject);
                        CostSkillNeedResource(target);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(subject);
                        UnitEndAction(target);  
                    }
                    else if (subjectDamageRate > targetDamageRate)
                    {
                        AddCounterBuff(target, subject);
                        if (subject.CheckReleaseSkillEnough())
                        {
                            CostSkillNeedResource(subject);
                            CalculateSkillDamageLogic(subject, target, ref subjectParamModel, ref targetParamModel);
                            TriggerReleaseSkillActionMoment(subject, subjectParamModel);
                            TriggerAfterUnderActionMoment(target, targetParamModel);
                            if (target.GetBeCounter())
                            {
                                CurrentRecordModel.SetTriggerCounterBuff(target.EntityID);
                                //被打破招 提前触发直到下次行动前扳机
                                RemoveBeforeNextActionEffect(target);
                                CostSkillNeedResource(target);
                                TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.BeCounter);
                                UnitEndAction(target);
                            }
                            TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(subject);
                        }
                        else
                        {
                            CostSkillNeedResource(subject);
                            TriggerAfterUnderActionMoment(target, targetParamModel);
                            TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(subject);
                        }
                    }
                    else
                    {
                        CostSkillNeedResource(subject);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(subject);
                    }
                }
                else if (subjectReleaseSkill)
                {
                    TriggerAfterClashMoment(subject, subjectParamModel);
                    TriggerAfterClashMoment(target, targetParamModel);
                    
                    AddCounterBuff(target, subject);
                    if (subject.CheckReleaseSkillEnough())
                    {
                        CostSkillNeedResource(subject);
                        CalculateSkillDamageLogic(subject, target, ref subjectParamModel, ref targetParamModel);
                        TriggerReleaseSkillActionMoment(subject, subjectParamModel);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        if (target.GetBeCounter())
                        {
                            CurrentRecordModel.SetTriggerCounterBuff(target.EntityID);
                            //被打破招 提前触发直到下次行动前扳机
                            RemoveBeforeNextActionEffect(target);
                            CostSkillNeedResource(target);
                            TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.BeCounter);
                            UnitEndAction(target);
                        }
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(subject);
                    }
                    else
                    {
                        CostSkillNeedResource(subject);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(subject);
                    }
                }
                else
                {
                    TriggerAfterClashMoment(subject, subjectParamModel);
                    TriggerAfterClashMoment(target, targetParamModel);
                    TriggerAfterUnderActionMoment(target, targetParamModel);
                    TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                    UnitEndAction(subject);
                }
            }
            else if (clashType == BattleClashType.DoubleClash)
            {
                Debug($"{subject.EntityID} : 双向交锋 : {target.EntityID}");
                var clashModel = CurrentRecordModel as DoubleClashRecordModel;
                TriggerBeforeClashMoment(subject, subjectParamModel);
                TriggerBeforeClashMoment(target, targetParamModel);
                var subjectReleaseSkill = subject.CheckReleaseSkillEnough();
                var targetReleaseSkill = target.CheckReleaseSkillEnough();
                
                clashModel.CheckSubjectCostInClash = subjectReleaseSkill;
                clashModel.CheckTargetCostInClash = targetReleaseSkill;
                if (subjectReleaseSkill && targetReleaseSkill)
                {
                    var subjectDamageRate = subject.GetSkillDamageRateFight();
                    var targetDamageRate = target.GetSkillDamageRateFight();
                    
                    clashModel.SetInClashSkillDamageRate(subject.EntityID, subjectDamageRate);
                    clashModel.SetInClashSkillDamageRate(target.EntityID, targetDamageRate);
                    
                    TriggerAfterClashMoment(subject, subjectParamModel);
                    TriggerAfterClashMoment(target, targetParamModel);
                    
                    if (Math.Abs(subjectDamageRate - targetDamageRate) <= 0.001f)
                    {
                        CostSkillNeedResource(subject);
                        CostSkillNeedResource(target);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        TriggerAfterUnderActionMoment(subject, subjectParamModel);
                        TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(subject);
                        UnitEndAction(target);
                    }
                    else if (subjectDamageRate > targetDamageRate)
                    {
                        AddCounterBuff(target, subject);
                        if (subject.CheckReleaseSkillEnough())
                        {
                            CostSkillNeedResource(subject);
                            CalculateSkillDamageLogic(subject, target, ref subjectParamModel, ref targetParamModel);
                            TriggerReleaseSkillActionMoment(subject, subjectParamModel);
                            TriggerAfterUnderActionMoment(target, targetParamModel);
                            if (target.GetBeCounter())
                            {
                                CurrentRecordModel.SetTriggerCounterBuff(target.EntityID);
                                CostSkillNeedResource(target);
                                TriggerAfterUnderActionMoment(subject, subjectParamModel);
                                TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.BeCounter);
                                UnitEndAction(target);
                            }
                            TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(subject);
                        }
                        else
                        {
                            CostSkillNeedResource(subject);
                            TriggerAfterUnderActionMoment(target, targetParamModel);
                            TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(subject);
                        }

                        if (!target.GetBeCounter())
                        {
                            CostSkillNeedResource(target);
                            if (target.CheckReleaseSkillEnough())
                            {
                                CalculateSkillDamageLogic(target, subject, ref targetParamModel, ref subjectParamModel);
                                TriggerReleaseSkillActionMoment(target, targetParamModel);
                            }
                            TriggerAfterUnderActionMoment(subject, subjectParamModel);
                            TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(target);
                        }
                    }
                    else
                    {
                        AddCounterBuff(subject, target);
                        if (target.CheckReleaseSkillEnough())
                        {
                            CostSkillNeedResource(target);
                            CalculateSkillDamageLogic(target, subject, ref targetParamModel, ref subjectParamModel);
                            TriggerReleaseSkillActionMoment(target, targetParamModel);
                            TriggerAfterUnderActionMoment(subject, subjectParamModel);
                            if (subject.GetBeCounter())
                            {
                                CurrentRecordModel.SetTriggerCounterBuff(subject.EntityID);
                                CostSkillNeedResource(subject);
                                TriggerAfterUnderActionMoment(target, subjectParamModel);
                                TriggerAfterActionMoment(subject, targetParamModel, SkillRemoveMomentType.BeCounter);
                                UnitEndAction(subject);
                            }
                            TriggerAfterActionMoment(target, subjectParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(target);
                        }
                        else
                        {
                            CostSkillNeedResource(target);
                            TriggerAfterUnderActionMoment(subject, targetParamModel);
                            TriggerAfterActionMoment(target, subjectParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(target);
                        }

                        if (!subject.GetBeCounter())
                        {
                            CostSkillNeedResource(subject);
                            if (subject.CheckReleaseSkillEnough())
                            {
                                CalculateSkillDamageLogic(subject, target, ref subjectParamModel, ref targetParamModel);
                                TriggerReleaseSkillActionMoment(subject, subjectParamModel);
                            }
                            TriggerAfterUnderActionMoment(target, targetParamModel);
                            TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(subject);
                        }
                    }
                }
                else if (subjectReleaseSkill)
                {
                    TriggerAfterClashMoment(subject, subjectParamModel);
                    TriggerAfterClashMoment(target, targetParamModel);
                    AddCounterBuff(target, subject);
                    if (subject.CheckReleaseSkillEnough())
                    {
                        CostSkillNeedResource(subject);
                        CalculateSkillDamageLogic(subject, target, ref subjectParamModel, ref targetParamModel);
                        TriggerReleaseSkillActionMoment(subject, subjectParamModel);
                        CostSkillNeedResource(target);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        TriggerAfterUnderActionMoment(subject, subjectParamModel);
                        if (target.GetBeCounter())
                        {
                            CurrentRecordModel.SetTriggerCounterBuff(target.EntityID);
                            TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.BeCounter);
                        }
                        else
                        {
                            TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                        }
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(target);
                        UnitEndAction(subject);
                    }
                    else
                    {
                        CostSkillNeedResource(subject);
                        CostSkillNeedResource(target);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        TriggerAfterUnderActionMoment(subject, subjectParamModel);
                        TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(target);
                        UnitEndAction(subject);
                    }
                }
                else if (targetReleaseSkill)
                {
                    TriggerAfterClashMoment(subject, subjectParamModel);
                    TriggerAfterClashMoment(target, targetParamModel);
                    AddCounterBuff(subject, target);
                    if (target.CheckReleaseSkillEnough())
                    {
                        CostSkillNeedResource(target);
                        CalculateSkillDamageLogic(target, subject, ref targetParamModel, ref subjectParamModel);
                        TriggerReleaseSkillActionMoment(target, targetParamModel);
                        CostSkillNeedResource(subject);
                        TriggerAfterUnderActionMoment(subject, subjectParamModel);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        if (subject.GetBeCounter())
                        {
                            CurrentRecordModel.SetTriggerCounterBuff(subject.EntityID); 
                            TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.BeCounter);
                        }
                        else
                        {
                            TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        }
                        TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(subject);
                        UnitEndAction(target);
                    }
                    else
                    {
                        CostSkillNeedResource(subject);
                        CostSkillNeedResource(target);
                        TriggerAfterUnderActionMoment(subject, subjectParamModel);
                        TriggerAfterUnderActionMoment(target, targetParamModel);
                        TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(subject);
                        UnitEndAction(target);
                    }
                }
                else
                {
                    TriggerAfterClashMoment(subject, subjectParamModel);
                    TriggerAfterClashMoment(target, targetParamModel);
                    CostSkillNeedResource(subject);
                    CostSkillNeedResource(target);
                    TriggerAfterUnderActionMoment(subject, subjectParamModel);
                    TriggerAfterUnderActionMoment(target, targetParamModel);
                    TriggerAfterActionMoment(subject, subjectParamModel, SkillRemoveMomentType.AfterAction);
                    TriggerAfterActionMoment(target, targetParamModel, SkillRemoveMomentType.AfterAction);
                    UnitEndAction(subject);
                    UnitEndAction(target);
                }
            }
            
            AddBattleRecordModel(CurrentRecordModel);
            
            PoolManager.RecycleClass(subjectParamModel);
            PoolManager.RecycleClass(targetParamModel);
            unitBeChooseKillingSkill.Clear();
        }

        foreach (var unit in OutActionUnits)
        {
            BattleLogicBehaviourManager.BattleBehaviourRes.Remove(unit.EntityID);
        }
        
        foreach (var actionUnit in OutActionUnits)
        {
            actionUnit.OneActionWheelEnd();
        }
        
        MessageManager.DispatchMsg<BattleStartActEventModel>(null);
    }

    private void BeforeActionJumpByResource(BattleUnit subject, BattleUnit target)
    {
        var model = PoolManager.GetClass<SingleActionRecordModel>();
        model.SubjectID = subject.EntityID;
        model.TargetID = target.EntityID;
        model.CheckSubjectCostPullFight = false;
        AddBattleRecordModel(model);
        Debug($"{subject.EntityID} : 资源不足  目标 : {target.EntityID}");
    }
    
    private void BeforeActionJumpByBeCounter(BattleUnit subject, BattleUnit target)
    {
        var model = PoolManager.GetClass<SingleActionRecordModel>();
        model.SubjectID = subject.EntityID;
        model.TargetID = target.EntityID;
        model.CheckSubjectBeCounter = true;
        AddBattleRecordModel(model);
        Debug($"{subject.EntityID} : 被破招了 目标 : {target.EntityID}");
    }

    private void AddBattleRecordModel(BattleRecordModel recordModel) =>
        BattleRecordManager.AddBattleRecordModel(recordModel);

    private void CostSkillNeedResource(BattleUnit unit)
    {
        var (gangQiCost, xuanQiCost, keyCost) = unit.CostSkillNeedResource();
        CurrentRecordModel.SetGangQiCost(unit.EntityID, gangQiCost);
        CurrentRecordModel.SetXuanQiCost(unit.EntityID, xuanQiCost);
        CurrentRecordModel.SetKeyCost(unit.EntityID, keyCost);
    }

    private void CalculateSkillDamageLogic(BattleUnit attacker, BattleUnit hit, ref DamageParamModel attackModel, ref DamageParamModel hitModel)
    {
        CurrentRecordModel.SetReleaseSkillSuccess(attacker.EntityID);
        var skillType = attacker.GetSkillType();
        var damageRate = attacker.GetSkillDamageRateSum();
        var damageType = attacker.GetSkillDamageType();
        var damageSource = BattleSource.Skill;
        var damageValue = attacker.GetSkillKillDamageValue(hit, damageType, damageSource, damageRate);
        attackModel.AttackSkillType = skillType;
        hitModel.HitSkillType = skillType;
        attackModel.AttackDamageType = damageType;
        hitModel.HitDamageType = damageType;
        attackModel.AttackSource = damageSource;
        hitModel.HitSource = damageSource;
        attackModel.AttackDamageValue = damageValue;
        hitModel.HitDamageValue = damageValue;
        hit.BeDamage(ref hitModel);
        attackModel.AttackHpValue = hitModel.HitHpValue;
        attackModel.AttackShieldValue = hitModel.HitShieldValue;

        //添加表现
        CurrentRecordModel.SetSkillID(attacker.EntityID, attacker.GetSkillID());
        CurrentRecordModel.SetSkillType(attacker.EntityID, skillType);
        CurrentRecordModel.SetSkillDamageRateDefault(attacker.EntityID, attacker.GetSkillDamageRate());
        CurrentRecordModel.SetSkillDamageRateFinal(attacker.EntityID, attacker.GetSkillDamageRateSum());
        CurrentRecordModel.SetBattleSource(attacker.EntityID, damageSource);
        CurrentRecordModel.SetDamageType(attacker.EntityID, damageType);
        CurrentRecordModel.SetDamageValue(attacker.EntityID, damageValue);
        CurrentRecordModel.SetAttackHpValue(attacker.EntityID, attackModel.AttackHpValue);
        CurrentRecordModel.SetAttackShieldValue(attacker.EntityID, attackModel.AttackShieldValue);
    }

    private void UnitAddAction(int entityID)
    {
        var unit = BattleManager.GetUnit(entityID);
        
        if (!InActionUnits.Contains(unit))
        {
            InActionUnits.Add(unit);
        }

        if (battleBehaviours.All(behaviour => behaviour.SubjectID != unit.EntityID))
        {
            battleBehaviours.Add(BattleLogicBehaviourManager.GetBattleBehaviour(unit.EntityID));
        }
        
        TriggerActionWheelStartMoment(unit);
    }
    
    private void UnitEndAction(BattleUnit unit)
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
        
        unit.ReduceActionTimes();
    }

    private void AddCounterBuff(BattleUnit target, BattleUnit spellCaster)
    {
        if (BattleBuffManager.AddBuff(target, GameConst.Battle.CounterBuffID, spellCaster, 1, null) != null);
        {
            CurrentRecordModel.SetAddCounterBuff(target.EntityID);
        }
    }
    
    /// <summary>
    /// 息开始扳机
    /// </summary>
    /// <param name="unit"></param>
    private void TriggerActionWheelStartMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.ActionWheelStart();
        }
    }

    /// <summary>
    /// 行动前扳机
    /// </summary>
    /// <param name="unit"></param>
    private void TriggerBeforeActionMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.BeforeAction();
        }
    }
    
    /// <summary>
    /// 受到行动前扳机
    /// </summary>
    /// <param name="unit"></param>
    private void TriggerBeforeUnderActionMoment(BattleUnit unit)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.BeforeUnderAction();
        }
    }

    /// <summary>
    /// 交锋前
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="model"></param>
    private void TriggerBeforeClashMoment(BattleUnit unit, DamageParamModel model)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.BeforeClash(model);
        }
    }

    /// <summary>
    /// 交锋后
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="model"></param>
    private void TriggerAfterClashMoment(BattleUnit unit, DamageParamModel model)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.AfterClash(model);
        }
    }

    /// <summary>
    /// 技能释放成功
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="model"></param>
    private void TriggerReleaseSkillActionMoment(BattleUnit unit, DamageParamModel model)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.ReleaseSkillAction(model);
        }
    }

    /// <summary>
    /// 受到行动后
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="model"></param>
    private void TriggerAfterUnderActionMoment(BattleUnit unit, DamageParamModel model)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.AfterUnderAction(model);
        }
    }

    /// <summary>
    /// 行动后
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="model"></param>
    /// <param name="type"></param>
    private void TriggerAfterActionMoment(BattleUnit unit, DamageParamModel model, SkillRemoveMomentType type)
    {
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.AfterAction(model);
        }
     
        unit.TryRemoveUseSkill(type);
    }

    private void RemoveBeforeNextActionEffect(BattleUnit unit)
    {
        unit.TryRemoveUseSkill(SkillRemoveMomentType.BeforeNextAction);
    }
}
