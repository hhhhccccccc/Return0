
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
    public override void Handle(BattleOneActionWheelLogicCalculateEventModel eventModel)
    {
        //触发每一息开始的扳机
        TriggerEveryActionWheelStart();
        BattleLogicStateManager.SetAfterStartActionWheel(true);
        BattleLogicStateManager.SetBattleState(BattleState.ActionWheelLogicCalculate);
        BattleLogicStateManager.RegisterAddUnitToNowLogicCalculate(UnitAddAction);
        TriggerSelfActionWheelStart(eventModel.ActionWheelUnit);
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
            var self = sortList.First();
            
            var selfBehaviour = battleBehaviours.First(behaviour => behaviour.SubjectID == self.EntityID);
            var other = BattleManager.GetUnit(selfBehaviour.TargetID);
            
            //移除下次行动前的效果
            RemoveBeforeNextActionEffect(self);
            //如果不满足招式释放条件(气)则直接跳过行动
            if (!self.CheckReleaseSkillEnough())
            {
                UnitEndAction(self);
                BeforeActionJumpByResource(self, other);
                continue;
            }
            
            //招式是否被打没掉
            if (self.GetBeCounter())
            {
                UnitEndAction(self);
                BeforeActionJumpByBeCounter(self, other);
                continue;
            }
            
            TriggerBeforeActionMoment(self);
            TriggerBeforeUnderActionMoment(other);
            
            var clashType = BattleClashType.None;
            var skillIsKillingStyle = self.SkillIsKillingStyle(); 
            //如果技能是非杀招 或者 为杀式但受击者在本息不存在行动 或者 为杀式但受击者本息已行动 或者 为杀式但受击者没有资源参与交锋 （则为单方面行动）
            if (!skillIsKillingStyle || InActionUnits.All(u => u.EntityID != selfBehaviour.TargetID) || !other.CheckReleaseSkillEnough()) //单方面行动
            {
                clashType = BattleClashType.SingleAction;
            }
            //如果是杀招 且 B在本息行动但还未行动 且不互相为目标 为单方面交锋 否则 为双向交锋
            else if (InActionUnits.Any(u => u.EntityID == selfBehaviour.TargetID))
            {
                var otherBehaviour = battleBehaviours.First(behaviour => behaviour.SubjectID == selfBehaviour.TargetID);
                clashType = (otherBehaviour.TargetID == self.EntityID && other.SkillIsKillingStyle()) ? BattleClashType.DoubleClash : BattleClashType.SingleClash;
            }

            UnitTriggerBeforeActionMomentEventModel(self, other, clashType);
            UnitTriggerBeforeUnderActionMomentEventModel(self, other, clashType);
            
            var model = PoolManager.GetClass<DamageParamModel>();
            model.BattleClashType = clashType;
            model.SelfID = self.EntityID;
            model.OtherID = other.EntityID;
            //如果是双向交锋 对方移除下次行动前的效果
            if (clashType == BattleClashType.DoubleClash)
            {
                RemoveBeforeNextActionEffect(other);
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

            CurrentRecordModel.SubjectID = self.EntityID;
            CurrentRecordModel.TargetID = other.EntityID;
            //添加行动前的扳机效果  
            BattleRecordManager.SetCurrentAndCacheRecordModel(CurrentRecordModel);
            CurrentRecordModel.CheckSubjectCostPullFight = true;
            if (!self.CheckReleaseSkillEnough())
            {
                CurrentRecordModel.CheckSubjectCostGenerateAction = false;
                CostSkillNeedResource(self, model);
                TriggerAfterUnderActionMoment(other, model);
                TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                UnitEndAction(self);
                AddBattleRecordModel(CurrentRecordModel);
                continue;
            }
            
            CurrentRecordModel.CheckSubjectCostGenerateAction = true;
            
            if (clashType == BattleClashType.SingleAction)
            {
                Debug($"{self.EntityID} : 单方面行动 : {other.EntityID}");
                var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                CalculateSkillDamageLogic(self, other, ref model);
                TriggerReleaseSkillActionMoment(self, model);
                TriggerAfterUnderActionMoment(other, model);
                TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                UnitEndAction(self);
            }
            else if (clashType == BattleClashType.SingleClash)
            {
                Debug($"{self.EntityID} : 单向交锋 : {other.EntityID}");
                var clashModel = CurrentRecordModel as SingleClashRecordModel;
                TriggerBeforeClashMoment(self, model);
                TriggerBeforeClashMoment(other, model);
                var subjectReleaseSkill = self.CheckReleaseSkillEnough();
                var targetReleaseSkill = other.CheckReleaseSkillEnough();

                clashModel.CheckSubjectCostInClash = subjectReleaseSkill;
                clashModel.CheckTargetCostInClash = targetReleaseSkill;
                
                if (subjectReleaseSkill && targetReleaseSkill)
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    
                    clashModel.SetInClashSkillDamageWelly(self.EntityID, subjectDamageWelly);
                    clashModel.SetInClashSkillDamageWelly(other.EntityID, targetDamageWelly);
                    
                    var (selfClashState, otherClashState) = CheckClashState(model, self, other, subjectDamageWelly, targetDamageWelly);
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    
                    if (!selfClashState && !otherClashState)//都失败
                    {
                        CostSkillNeedResource(self, model);
                        CostSkillNeedResource(other, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(self);
                        UnitEndAction(other);  
                    }
                    else if (selfClashState)
                    {
                        AddCounterBuff(other, self);
                        if (self.CheckReleaseSkillEnough())
                        {
                            CalculateSkillDamageLogic(self, other, ref model);
                            TriggerReleaseSkillActionMoment(self, model);
                            TriggerAfterUnderActionMoment(other, model);
                            if (other.GetBeCounter())
                            {
                                CurrentRecordModel.SetTriggerCounterBuff(other.EntityID);
                                //被打破招 提前触发直到下次行动前扳机
                                RemoveBeforeNextActionEffect(other);
                                CostSkillNeedResource(other, model);
                                TriggerAfterActionMoment(other, model, SkillRemoveMomentType.BeCounter);
                                UnitEndAction(other);
                            }
                            TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(self);
                        }
                        else
                        {
                            CostSkillNeedResource(self, model);
                            TriggerAfterUnderActionMoment(other, model);
                            TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(self);
                        }
                    }
                    else
                    {
                        CostSkillNeedResource(self, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(self);
                    }
                }
                else if (subjectReleaseSkill)
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);

                    var selfClashState = true;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    
                    AddCounterBuff(other, self);
                    if (self.CheckReleaseSkillEnough())
                    {
                        CalculateSkillDamageLogic(self, other, ref model);
                        TriggerReleaseSkillActionMoment(self, model);
                        TriggerAfterUnderActionMoment(other, model);
                        if (other.GetBeCounter())
                        {
                            CurrentRecordModel.SetTriggerCounterBuff(other.EntityID);
                            //被打破招 提前触发直到下次行动前扳机
                            RemoveBeforeNextActionEffect(other);
                            CostSkillNeedResource(other, model);
                            TriggerAfterActionMoment(other, model, SkillRemoveMomentType.BeCounter);
                            UnitEndAction(other);
                        }
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(self);
                    }
                    else
                    {
                        CostSkillNeedResource(self, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(self);
                    }
                }
                else
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var selfClashState = false;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    TriggerAfterUnderActionMoment(other, model);
                    TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                    UnitEndAction(self);
                }
            }
            else if (clashType == BattleClashType.DoubleClash)
            {
                Debug($"{self.EntityID} : 双向交锋 : {other.EntityID}");
                var clashModel = CurrentRecordModel as DoubleClashRecordModel;
                TriggerBeforeClashMoment(self, model);
                TriggerBeforeClashMoment(other, model);
                var subjectReleaseSkill = self.CheckReleaseSkillEnough();
                var targetReleaseSkill = other.CheckReleaseSkillEnough();
                
                clashModel.CheckSubjectCostInClash = subjectReleaseSkill;
                clashModel.CheckTargetCostInClash = targetReleaseSkill;
                if (subjectReleaseSkill && targetReleaseSkill)
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    
                    clashModel.SetInClashSkillDamageWelly(self.EntityID, subjectDamageWelly);
                    clashModel.SetInClashSkillDamageWelly(other.EntityID, targetDamageWelly);
                    
                    var (selfClashState, otherClashState) = CheckClashState(model, self, other, subjectDamageWelly, targetDamageWelly);
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);

                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    
                    if (!selfClashState && !otherClashState)
                    {
                        CostSkillNeedResource(self, model);
                        CostSkillNeedResource(other, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterUnderActionMoment(self, model);
                        TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(self);
                        UnitEndAction(other);
                    }
                    else if (selfClashState)
                    {
                        AddCounterBuff(other, self);
                        if (self.CheckReleaseSkillEnough())
                        {
                            CalculateSkillDamageLogic(self, other, ref model);
                            TriggerReleaseSkillActionMoment(self, model);
                            TriggerAfterUnderActionMoment(other, model);
                            if (other.GetBeCounter())
                            {
                                CurrentRecordModel.SetTriggerCounterBuff(other.EntityID);
                                CostSkillNeedResource(other, model);
                                TriggerAfterUnderActionMoment(self, model);
                                TriggerAfterActionMoment(other, model, SkillRemoveMomentType.BeCounter);
                                UnitEndAction(other);
                            }
                            TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(self);
                        }
                        else
                        {
                            CostSkillNeedResource(self, model);
                            TriggerAfterUnderActionMoment(other, model);
                            TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(self);
                        }

                        if (!other.GetBeCounter())
                        {
                            if (other.CheckReleaseSkillEnough())
                            {
                                CalculateSkillDamageLogic(other, self, ref model);
                                TriggerReleaseSkillActionMoment(other, model);
                            }
                            else
                            {
                                CostSkillNeedResource(other, model);
                            }
                            TriggerAfterUnderActionMoment(self, model);
                            TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(other);
                        }
                    }
                    else
                    {
                        AddCounterBuff(self, other);
                        if (other.CheckReleaseSkillEnough())
                        {
                            CalculateSkillDamageLogic(other, self, ref model);
                            TriggerReleaseSkillActionMoment(other, model);
                            TriggerAfterUnderActionMoment(self, model);
                            if (self.GetBeCounter())
                            {
                                CurrentRecordModel.SetTriggerCounterBuff(self.EntityID);
                                CostSkillNeedResource(self, model);
                                TriggerAfterUnderActionMoment(other, model);
                                TriggerAfterActionMoment(self, model, SkillRemoveMomentType.BeCounter);
                                UnitEndAction(self);
                            }
                            TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(other);
                        }
                        else
                        {
                            CostSkillNeedResource(other, model);
                            TriggerAfterUnderActionMoment(self, model);
                            TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(other);
                        }

                        if (!self.GetBeCounter())
                        {
                            if (self.CheckReleaseSkillEnough())
                            {
                                CalculateSkillDamageLogic(self, other, ref model);
                                TriggerReleaseSkillActionMoment(self, model);
                            }
                            else
                            {
                                CostSkillNeedResource(self, model);
                            }
                            TriggerAfterUnderActionMoment(other, model);
                            TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                            UnitEndAction(self);
                        }
                    }
                }
                else if (subjectReleaseSkill)
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var selfClashState = true;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    AddCounterBuff(other, self);
                    if (self.CheckReleaseSkillEnough())
                    {
                        CalculateSkillDamageLogic(self, other, ref model);
                        TriggerReleaseSkillActionMoment(self, model);
                        CostSkillNeedResource(other, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterUnderActionMoment(self, model);
                        if (other.GetBeCounter())
                        {
                            CurrentRecordModel.SetTriggerCounterBuff(other.EntityID);
                            TriggerAfterActionMoment(other, model, SkillRemoveMomentType.BeCounter);
                        }
                        else
                        {
                            TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                        }
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(other);
                        UnitEndAction(self);
                    }
                    else
                    {
                        CostSkillNeedResource(self, model);
                        CostSkillNeedResource(other, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterUnderActionMoment(self, model);
                        TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(other);
                        UnitEndAction(self);
                    }
                }
                else if (targetReleaseSkill)
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var selfClashState = false;
                    var otherClashState = true;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    AddCounterBuff(self, other);
                    if (other.CheckReleaseSkillEnough())
                    {
                        CalculateSkillDamageLogic(other, self, ref model);
                        TriggerReleaseSkillActionMoment(other, model);
                        CostSkillNeedResource(self, model);
                        TriggerAfterUnderActionMoment(self, model);
                        TriggerAfterUnderActionMoment(other, model);
                        if (self.GetBeCounter())
                        {
                            CurrentRecordModel.SetTriggerCounterBuff(self.EntityID); 
                            TriggerAfterActionMoment(self, model, SkillRemoveMomentType.BeCounter);
                        }
                        else
                        {
                            TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        }
                        TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(self);
                        UnitEndAction(other);
                    }
                    else
                    {
                        CostSkillNeedResource(self, model);
                        CostSkillNeedResource(other, model);
                        TriggerAfterUnderActionMoment(self, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                        TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                        UnitEndAction(self);
                        UnitEndAction(other);
                    }
                }
                else
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                    var selfClashState = false;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetFinalDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    CostSkillNeedResource(self, model);
                    CostSkillNeedResource(other, model);
                    TriggerAfterUnderActionMoment(self, model);
                    TriggerAfterUnderActionMoment(other, model);
                    TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                    TriggerAfterActionMoment(other, model, SkillRemoveMomentType.AfterAction);
                    UnitEndAction(self);
                    UnitEndAction(other);
                }
            }
            
            AddBattleRecordModel(CurrentRecordModel);
            
            PoolManager.RecycleClass(model);
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

    private void TriggerEveryActionWheelStart()
    {
        BattleRecordManager.SetMomentType(BattleMomentType.EveryActionWheelStart);
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.EveryActionWheelStart();
            }
        }

        MessageManager.DispatchMsg<TriggerEveryActionWheelStartEventModel>(null);
    }
    
    private void TriggerSelfActionWheelStart(List<int> unitList)
    {
        BattleRecordManager.SetMomentType(BattleMomentType.ActionWheelStart);
        foreach (var entityID in unitList)
        {
            UnitAddAction(entityID);
        }
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

    private void CostSkillNeedResource(BattleUnit unit, DamageParamModel model)
    {
        var (gangQiCost, xuanQiCost, keyCost) = unit.CostSkillNeedResource();
        
        model.SetGangQiCost(unit.EntityID, gangQiCost);
        model.SetXuanQiCost(unit.EntityID, xuanQiCost);
        model.SetKeyCost(unit.EntityID, keyCost);
        
        CurrentRecordModel.SetGangQiCost(unit.EntityID, gangQiCost);
        CurrentRecordModel.SetXuanQiCost(unit.EntityID, xuanQiCost);
        CurrentRecordModel.SetKeyCost(unit.EntityID, keyCost);
    }

    private void CalculateSkillDamageLogic(BattleUnit attacker, BattleUnit hit, ref DamageParamModel model)
    {
        CurrentRecordModel.SetReleaseSkillSuccess(attacker.EntityID);
        var skillID = attacker.GetSkill().SkillID;
        var variantID = attacker.GetSkill().VariantID;
        var skillType = attacker.GetSkillType();
        var damageWelly = attacker.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
        var damageType = attacker.GetSkillDamageType();
        var damageSource = BattleSource.Skill;
        model.SetSkillID(attacker.EntityID, skillID);
        model.SetVariantID(attacker.EntityID, variantID);
        model.SetSkillType(attacker.EntityID, skillType);
        model.SetDamageType(attacker.EntityID, damageType);
        model.SetBattleSource(attacker.EntityID, damageSource);
        
        var (truthDamage, reduceHp, reduceShield, reduceArmor) = attacker.GetSkillDamageValue(hit, damageType, damageSource, damageWelly, model);
        model.SetTruthDamageValue(attacker.EntityID, truthDamage);
        model.SetHpValue(attacker.EntityID, reduceHp);
        model.SetShieldValue(attacker.EntityID, reduceShield);
        model.SetArmorValue(attacker.EntityID, reduceArmor);
        
        //雨割   扣除体上限
        if ((attacker.BattleChangeModelManager.CheckHasMethod(GameConst.Battle.HeartMethod10136) ||
             hit.BattleChangeModelManager.CheckHasMethod(GameConst.Battle.HeartMethod10136))
            && damageType == DamageType.Direct && BattleLogicStateManager.BattleWeatherType == WeatherType.Rain)
        {
            model.SetDamageReduceMaxHp(attacker.EntityID, true);
        }
        
        //重新计算
        if (hit.BattleChangeModelManager.CheckReCalculateDamage(model))
        {
            (truthDamage, reduceHp, reduceShield, reduceArmor) = attacker.GetSkillDamageValue(hit, damageType, damageSource, damageWelly, model);
            model.SetTruthDamageValue(attacker.EntityID, truthDamage);
            model.SetHpValue(attacker.EntityID, reduceHp);
            model.SetShieldValue(attacker.EntityID, reduceShield);
            model.SetArmorValue(attacker.EntityID, reduceArmor);
        }
        
        attacker.BattleChangeModelManager.BeforeAttack(model);
        hit.BeDamage(ref model);
        CostSkillNeedResource(attacker, model);
        
        //添加表现
        CurrentRecordModel.SetSkillID(attacker.EntityID, attacker.GetSkillID());
        CurrentRecordModel.SetSkillType(attacker.EntityID, skillType);
        CurrentRecordModel.SetSkillDamageWellyDefault(attacker.EntityID, attacker.GetSkillDamageWelly(SkillDataGetType.DamageBase));
        CurrentRecordModel.SetSkillDamageWellyFinal(attacker.EntityID, damageWelly);
        CurrentRecordModel.SetBattleSource(attacker.EntityID, damageSource);
        CurrentRecordModel.SetDamageType(attacker.EntityID, damageType);
        CurrentRecordModel.SetTruthDamage(attacker.EntityID, model.GetSelfTruthDamageValue(attacker.EntityID));
        CurrentRecordModel.SetAttackHpValue(attacker.EntityID, model.GetSelfHpValue(attacker.EntityID));
        CurrentRecordModel.SetAttackShieldValue(attacker.EntityID, model.GetSelfShieldValue(attacker.EntityID));
        CurrentRecordModel.SetAttackShieldValue(attacker.EntityID,model.GetSelfArmorValue(attacker.EntityID));
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
        
        TriggerAfterSelfActionWheelStartMoment(unit);
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

        BattleLogicStateManager.TryAddRoundAlreadyActionUnit(unit.EntityID);
        unit.EndAction();
    }

    private void AddCounterBuff(BattleUnit target, BattleUnit spellCaster)
    {
        if (BattleBuffManager.AddBuff(target, GameConst.Battle.CounterBuffID, spellCaster, 1, null) != null);
        {
            CurrentRecordModel.SetAddCounterBuff(target.EntityID);
        }
    }
    
    /// <summary>
    /// 自己息开始扳机
    /// </summary>
    /// <param name="unit"></param>
    private void TriggerAfterSelfActionWheelStartMoment(BattleUnit unit)
    {
        BattleRecordManager.SetMomentType(BattleMomentType.ActionWheelStart);
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.SelfActionWheelStart();
        }
    }

    /// <summary>
    /// 行动前扳机
    /// </summary>
    /// <param name="unit"></param>
    private void TriggerBeforeActionMoment(BattleUnit unit)
    {
        BattleRecordManager.SetMomentType(BattleMomentType.BeforeAction);
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.BeforeAction();
        }
    }

    /// <summary>
    /// 行动前全局事件
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="clashType"></param>
    /// <param name="attacker"></param>
    private void UnitTriggerBeforeActionMomentEventModel(BattleUnit attacker, BattleUnit hit, BattleClashType clashType)
    {
        var model = PoolManager.GetClass<UnitTriggerBeforeActionMomentEventModel>();
        model.AttackerID = attacker.EntityID;
        model.HitID = hit.EntityID;
        model.ClashType = clashType;
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }
    
    /// <summary>
    /// 受到行动前扳机
    /// </summary>
    /// <param name="unit"></param>
    private void TriggerBeforeUnderActionMoment(BattleUnit unit)
    {
        BattleRecordManager.SetMomentType(BattleMomentType.BeforeAction);
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.BeforeUnderAction();
        }
    }

    /// <summary>
    /// 受到行动前全局事件
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="hit"></param>
    /// <param name="clashType"></param>
    private void UnitTriggerBeforeUnderActionMomentEventModel(BattleUnit attacker, BattleUnit hit, BattleClashType clashType)
    {
        var model = PoolManager.GetClass<UnitTriggerBeforeUnderActionMomentEventModel>();
        model.AttackerID = attacker.EntityID;
        model.HitID = hit.EntityID;
        model.ClashType = clashType;
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }


    /// <summary>
    /// 行动后全局事件
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="paramModel"></param>
    private void UnitTriggerAfterActionMomentEventModel(BattleUnit attacker, DamageParamModel paramModel)
    {
        var model = PoolManager.GetClass<UnitTriggerAfterActionMomentEventModel>();
        model.EntityID = attacker.EntityID;
        model.SkillID =  paramModel.GetSelfSkillID(attacker.EntityID);
        model.UseSuccess = paramModel.GetSelfUseSuccess(attacker.EntityID);
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }

    /// <summary>
    /// 交锋前
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="model"></param>
    private void TriggerBeforeClashMoment(BattleUnit unit, DamageParamModel model)
    {
        BattleRecordManager.SetMomentType(BattleMomentType.BeforeClash);
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
        BattleRecordManager.SetMomentType(BattleMomentType.AfterClash);
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
        BattleRecordManager.SetMomentType(BattleMomentType.ReleaseSkillAction);
        model.SetUseSuccess(unit.EntityID, true);
   
       
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.ReleaseSkillAction(model);
        }
        
        var eventModel = PoolManager.GetClass<UnitTriggerReleaseSkillActionEventModel>();
        eventModel.AttackerID = model.GetSelfID(unit.EntityID);
        eventModel.HitID = model.GetOtherID(unit.EntityID);
        MessageManager.DispatchMsg(eventModel);
        PoolManager.RecycleClass(eventModel);
    }

    /// <summary>
    /// 受到行动后
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="model"></param>
    private void TriggerAfterUnderActionMoment(BattleUnit unit, DamageParamModel model)
    {
        BattleRecordManager.SetMomentType(BattleMomentType.AfterAction);
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
        BattleRecordManager.SetMomentType(BattleMomentType.AfterAction);
        foreach (var moment in unit.GetBattleMoment())
        {
            moment.AfterAction(model);
        }

        UnitTriggerAfterActionMomentEventModel(unit, model);
        unit.TryRemoveUseSkill(type, model);
    }

    
    private void RemoveBeforeNextActionEffect(BattleUnit unit)
    {
        BattleRecordManager.SetMomentType(BattleMomentType.BeforeNextAction);
        unit.TryRemoveUseSkill(SkillRemoveMomentType.BeforeNextAction);
        unit.BattleChangeModelManager.RemoveBeforeNextAction();
    }

    private (bool, bool) CheckClashState(DamageParamModel model, BattleUnit self, BattleUnit other, float selfDamageWelly, float otherDamageWelly)
    {
        var selfClashState = false;
        var otherClashState = false;
        var isSame = Math.Abs(selfDamageWelly - otherDamageWelly) <= 0.001f;
        if (!isSame)
        {
            if (selfDamageWelly > otherDamageWelly)
            {
                selfClashState = true;
            }
            else
            {
                otherClashState = true;
            }
        }
        //对方失败先重置试试
        other.BattleChangeModelManager.ReCheckClashState(ref otherClashState, otherDamageWelly, selfDamageWelly);
        self.BattleChangeModelManager.ReCheckClashState(ref selfClashState, selfDamageWelly, otherDamageWelly);
        if (selfClashState)
        {
            otherClashState = false;
        }

        return (selfClashState, otherClashState);
    }

    private void SetClashState(DamageParamModel model, BattleUnit self, BattleUnit other, bool selfClashState, bool otherClashState)
    {
        model.SetClashState(self.EntityID, selfClashState);
        model.SetClashState(other.EntityID, otherClashState);
        
        self.AddSkillClashState(selfClashState);
        other.AddSkillClashState(otherClashState);
    }

    private void SetFinalDamageWelly(DamageParamModel model, BattleUnit self, BattleUnit other, float selfDamageWelly, float otherDamageWelly)
    {
        model.SetFinalDamageWelly(self.EntityID, selfDamageWelly);
        model.SetFinalDamageWelly(other.EntityID, otherDamageWelly);
    }
}
