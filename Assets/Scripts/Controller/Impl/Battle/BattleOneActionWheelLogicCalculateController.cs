
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
    public override void Handle(BattleOneActionWheelLogicCalculateEventModel eventModel)
    {
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            unit.ViewType = BattleMomentViewType.EveryActionWheelStart;
        }
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
            
            //如果是双向交锋 对方移除下次行动前的效果
            if (clashType == BattleClashType.DoubleClash)
            {
                RemoveBeforeNextActionEffect(other);
            }
            var recordModel = BattleRecordManager.NewRecordModel(clashType, self.EntityID, other.EntityID);
            var model = recordModel.DamageParamModel;
            recordModel.CheckSelfCostPullFight = true;
            if (!self.CheckReleaseSkillEnough())
            {
                recordModel.CheckSelfCostGenerateAction = false;
                CostSkillNeedResource(self, model);
                TriggerAfterUnderActionMoment(other, model);
                TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                UnitEndAction(self);
                continue;
            }
            
            recordModel.CheckSelfCostGenerateAction = true;
            
            if (clashType == BattleClashType.SingleAction)
            {
                Debug($"{self.EntityID} : 单方面行动 : {other.EntityID}");
                var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                CalculateSkillDamageLogic(self, other, ref model);
                TriggerReleaseSkillActionMoment(self, model);
                TriggerAfterUnderActionMoment(other, model);
                TriggerAfterActionMoment(self, model, SkillRemoveMomentType.AfterAction);
                UnitEndAction(self);
            }
            else if (clashType == BattleClashType.SingleClash)
            {
                Debug($"{self.EntityID} : 单向交锋 : {other.EntityID}");
                var clashModel = recordModel as SingleClashRecordModel;
                TriggerBeforeClashMoment(self, model);
                TriggerBeforeClashMoment(other, model);
                var subjectReleaseSkill = self.CheckReleaseSkillEnough();
                var targetReleaseSkill = other.CheckReleaseSkillEnough();

                clashModel.CheckSelfCostInClash = subjectReleaseSkill;
                clashModel.CheckOtherCostInClash = targetReleaseSkill;
                
                if (subjectReleaseSkill && targetReleaseSkill)
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    
                    var (selfClashState, otherClashState) = CheckClashState(model, self, other, subjectDamageWelly, targetDamageWelly);
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
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
                        AddCounterBuff(other, self, model);
                        if (self.CheckReleaseSkillEnough())
                        {
                            CalculateSkillDamageLogic(self, other, ref model);
                            TriggerReleaseSkillActionMoment(self, model);
                            TriggerAfterUnderActionMoment(other, model);
                            if (other.GetBeCounter())
                            {
                                model.SetBeTriggerCounterBuff(other.EntityID);
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
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);

                    var selfClashState = true;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    
                    AddCounterBuff(other, self, model);
                    if (self.CheckReleaseSkillEnough())
                    {
                        CalculateSkillDamageLogic(self, other, ref model);
                        TriggerReleaseSkillActionMoment(self, model);
                        TriggerAfterUnderActionMoment(other, model);
                        if (other.GetBeCounter())
                        {
                            model.SetBeTriggerCounterBuff(other.EntityID);
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
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var selfClashState = false;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
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
                var clashModel = recordModel as DoubleClashRecordModel;
                TriggerBeforeClashMoment(self, model);
                TriggerBeforeClashMoment(other, model);
                var subjectReleaseSkill = self.CheckReleaseSkillEnough();
                var targetReleaseSkill = other.CheckReleaseSkillEnough();
                
                clashModel.CheckSelfCostInClash = subjectReleaseSkill;
                clashModel.CheckOtherCostInClash = targetReleaseSkill;
                if (subjectReleaseSkill && targetReleaseSkill)
                {
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    
                    var (selfClashState, otherClashState) = CheckClashState(model, self, other, subjectDamageWelly, targetDamageWelly);
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);

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
                        AddCounterBuff(other, self, model);
                        if (self.CheckReleaseSkillEnough())
                        {
                            CalculateSkillDamageLogic(self, other, ref model);
                            TriggerReleaseSkillActionMoment(self, model);
                            TriggerAfterUnderActionMoment(other, model);
                            if (other.GetBeCounter())
                            {
                                model.SetBeTriggerCounterBuff(other.EntityID);
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
                        AddCounterBuff(self, other, model);
                        if (other.CheckReleaseSkillEnough())
                        {
                            CalculateSkillDamageLogic(other, self, ref model);
                            TriggerReleaseSkillActionMoment(other, model);
                            TriggerAfterUnderActionMoment(self, model);
                            if (self.GetBeCounter())
                            {
                                model.SetBeTriggerCounterBuff(self.EntityID);
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
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var selfClashState = true;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    AddCounterBuff(other, self, model);
                    if (self.CheckReleaseSkillEnough())
                    {
                        CalculateSkillDamageLogic(self, other, ref model);
                        TriggerReleaseSkillActionMoment(self, model);
                        CostSkillNeedResource(other, model);
                        TriggerAfterUnderActionMoment(other, model);
                        TriggerAfterUnderActionMoment(self, model);
                        if (other.GetBeCounter())
                        {
                            model.SetBeTriggerCounterBuff(other.EntityID);
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
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var selfClashState = false;
                    var otherClashState = true;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
                    TriggerAfterClashMoment(self, model);
                    TriggerAfterClashMoment(other, model);
                    AddCounterBuff(self, other, model);
                    if (other.CheckReleaseSkillEnough())
                    {
                        CalculateSkillDamageLogic(other, self, ref model);
                        TriggerReleaseSkillActionMoment(other, model);
                        CostSkillNeedResource(self, model);
                        TriggerAfterUnderActionMoment(self, model);
                        TriggerAfterUnderActionMoment(other, model);
                        if (self.GetBeCounter())
                        {
                            model.SetBeTriggerCounterBuff(self.EntityID); 
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
                    var subjectDamageWelly = self.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var targetDamageWelly = other.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
                    var selfClashState = false;
                    var otherClashState = false;
                    SetClashState(model, self, other, selfClashState, otherClashState);
                    SetDamageWelly(model, self, other, subjectDamageWelly, targetDamageWelly);
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
            
            //PoolManager.RecycleClass(model);
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
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            unit.AddLastActionWheelToNow(1);
            foreach (var moment in unit.BattleMomentManager.GetMoments())
            {
                moment.EveryActionWheelStart();
            }
        }

        MessageManager.DispatchMsg<TriggerEveryActionWheelStartEventModel>(null);
    }
    
    private void TriggerSelfActionWheelStart(List<int> unitList)
    {
        foreach (var entityID in unitList)
        {
            UnitAddAction(entityID);
        }
    }

    private void BeforeActionJumpByResource(BattleUnit subject, BattleUnit target)
    {
        var recordModel = BattleRecordManager.NewRecordModel(BattleClashType.SingleAction, subject.EntityID, target.EntityID);
        var model = recordModel.DamageParamModel;
        model.BattleClashType = BattleClashType.SingleAction;
        model.SetSelfID(subject.EntityID);
        model.SetOtherID(target.EntityID);
        recordModel.CheckSelfCostPullFight = false;
        Debug($"{subject.EntityID} : 资源不足  目标 : {target.EntityID}");
    }

    private void CostSkillNeedResource(BattleUnit unit, DamageParamModel model)
    {
        unit.ViewType = BattleMomentViewType.CostResource;
        var (gangQiCost, xuanQiCost, keyCost) = unit.CostSkillNeedResource();
        
        model.SetGangQiCost(unit.EntityID, gangQiCost);
        model.SetXuanQiCost(unit.EntityID, xuanQiCost);
        model.SetKeyCost(unit.EntityID, keyCost);
    }

    private void CalculateSkillDamageLogic(BattleUnit attacker, BattleUnit hit, ref DamageParamModel model)
    {
        model.SetReleaseSkillSuccess(attacker.EntityID);
        
        var skillID = attacker.GetSkill().SkillID;
        var variantID = attacker.GetSkill().VariantID;
        var skillType = attacker.GetSkillType();
        var damageWelly = attacker.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr);
        var damageType = attacker.GetSkillDamageType();
        var damageSource = BattleSource.Skill;
        model.SetSkillID(attacker.EntityID, skillID);
        model.SetVariantID(attacker.EntityID, variantID);
        model.SetSkillType(attacker.EntityID, skillType);
        model.SetDamageType(attacker.EntityID, damageType);
        model.SetBattleSource(attacker.EntityID, damageSource);
        
        var (truthDamage, reduceHp, reduceShield, reduceArmor) = attacker.GetSkillDamageValue(hit, damageType, damageSource, damageWelly, model);
        model.SetAttackTruthDamageValue(attacker.EntityID, truthDamage);
        model.SetAttackHpValue(attacker.EntityID, reduceHp);
        model.SetAttackShieldValue(attacker.EntityID, reduceShield);
        model.SetAttackArmorValue(attacker.EntityID, reduceArmor);
        
        //雨割   扣除体上限
        if ((attacker.BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10136) ||
             hit.BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10136))
            && damageType == DamageType.Direct && BattleLogicStateManager.BattleWeatherType == WeatherType.Rain)
        {
            model.SetDamageReduceMaxHp(attacker.EntityID, true);
        }
        
        //重新计算
        if (hit.BattleMomentManager.CheckReCalculateDamage(model))
        {
            (truthDamage, reduceHp, reduceShield, reduceArmor) = attacker.GetSkillDamageValue(hit, damageType, damageSource, damageWelly, model);
            model.SetAttackTruthDamageValue(attacker.EntityID, truthDamage);
            model.SetAttackHpValue(attacker.EntityID, reduceHp);
            model.SetAttackShieldValue(attacker.EntityID, reduceShield);
            model.SetAttackArmorValue(attacker.EntityID, reduceArmor);
        }
        
        attacker.BattleMomentManager.BeforeAttack(model);
        hit.BeDamage(ref model);
        CostSkillNeedResource(attacker, model);
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

    private void AddCounterBuff(BattleUnit target, BattleUnit spellCaster, DamageParamModel model)
    {
        if (BattleBuffManager.AddBuff(target, GameConst.Battle.CounterBuffID, spellCaster, 1, null) != null);
        {
            model.SetBeAddCounterBuff(target.EntityID);
        }
    }
    
    /// <summary>
    /// 自己息开始扳机
    /// </summary>
    /// <param name="unit"></param>
    private void TriggerAfterSelfActionWheelStartMoment(BattleUnit unit)
    {
        unit.ViewType = BattleMomentViewType.SelfActionWheelStart;
        foreach (var moment in unit.BattleMomentManager.GetMoments())
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
        unit.ViewType = BattleMomentViewType.BeforeAction;
        foreach (var moment in unit.BattleMomentManager.GetMoments())
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
        unit.ViewType = BattleMomentViewType.BeforeUnderAction;
        foreach (var moment in unit.BattleMomentManager.GetMoments())
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
        model.UseSuccess = paramModel.GetSelfSkillUseSuccess(attacker.EntityID);
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
        unit.ViewType = BattleMomentViewType.BeforeClash;
        foreach (var moment in unit.BattleMomentManager.GetMoments())
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
        unit.ViewType = BattleMomentViewType.AfterClash;
        foreach (var moment in unit.BattleMomentManager.GetMoments())
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
        model.SetUseSuccess(unit.EntityID, true);
        foreach (var moment in unit.BattleMomentManager.GetMoments())
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
        unit.ViewType = BattleMomentViewType.AfterUnderAction;
        foreach (var moment in unit.BattleMomentManager.GetMoments())
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
        unit.ViewType = BattleMomentViewType.AfterAction;
        foreach (var moment in unit.BattleMomentManager.GetMoments())
        {
            moment.AfterAction(model);
        }

        UnitTriggerAfterActionMomentEventModel(unit, model);
        unit.TryRemoveUseSkill(type, model);
    }
    
    private void RemoveBeforeNextActionEffect(BattleUnit unit)
    {
        unit.TryRemoveUseSkill(SkillRemoveMomentType.BeforeNextAction);
        unit.BattleMomentManager.RemoveBeforeNextAction();
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
        other.BattleMomentManager.ReCheckClashState(ref otherClashState, otherDamageWelly, selfDamageWelly);
        self.BattleMomentManager.ReCheckClashState(ref selfClashState, selfDamageWelly, otherDamageWelly);
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

    private void SetDamageWelly(DamageParamModel model, BattleUnit self, BattleUnit other, float selfDamageWelly, float otherDamageWelly)
    {
        model.SetDefaultDamageWelly(self.EntityID, self.GetSkillDamageWelly(SkillDataGetType.WellyRateBase));
        model.SetDefaultDamageWelly(other.EntityID, other.GetSkillDamageWelly(SkillDataGetType.WellyRateBase));
        
        model.SetFinalDamageWelly(self.EntityID, selfDamageWelly);
        model.SetFinalDamageWelly(other.EntityID, otherDamageWelly);
    }
}
