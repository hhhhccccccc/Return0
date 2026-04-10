using System;
using System.Collections;
using cfg;
using UnityEngine;
using Zenject;

public class DoubleClashRecordViewHandleModel : RecordViewHandleModel<DoubleClashRecordModel>
{
    private float SelfDefaultDamageWelly { get; set; }
    private float OtherDefaultDamageWelly { get; set; }
    private float SelfFinalDamageWelly { get; set; }
    private float OtherFinalDamageWelly { get; set; }
    private bool SelfInClasDamageRate { get; set; }
    private bool OtherInClasDamageRate { get; set; }

    protected override void InitData()
    {
        base.InitData();
        SelfDefaultDamageWelly = LogicModel.GetSelfDefaultWellyRate(SelfID);
        OtherDefaultDamageWelly = LogicModel.GetSelfDefaultWellyRate(OtherID);
        SelfFinalDamageWelly = LogicModel.GetSelfFinalWellyRate(SelfID);
        OtherFinalDamageWelly = LogicModel.GetSelfFinalWellyRate(OtherID);
        SelfInClasDamageRate = LogicModel.GetSelfClashState(SelfID);
        OtherInClasDamageRate = LogicModel.GetSelfClashState(OtherID);
    }
    
    protected override IEnumerator OnHandle()
    {
        var model = RecordModel;
        SetSettlementUI(model.SelfID, true);
        SetSettlementUI(model.OtherID, true);
        yield return GetWaitTimeModel(0.2f);
        SetSettlementDamageRateValue(SelfID, true, SelfDefaultDamageWelly);
        SetSettlementDamageRateValue(OtherID, true, OtherDefaultDamageWelly);
        
        yield return WaitMomentShow(
            model.GetQueue(SelfID, BattleMomentViewType.BeforeAction), 
            model.GetQueue(OtherID, BattleMomentViewType.BeforeUnderAction));
        
        yield return WaitMomentShow(
            model.GetQueue(SelfID, BattleMomentViewType.BeforeClash), 
            model.GetQueue(OtherID, BattleMomentViewType.BeforeClash));

        var SelfCostEnough = model.CheckSelfCostInClash;
        var OtherCostEnough = model.CheckOtherCostInClash;
        
        if (!SelfCostEnough)
        {
            SetSettlementUI(model.SelfID, false, "", delayClose: CloseSettlementDelay);
        }
        
        if (!OtherCostEnough)
        {
            SetSettlementUI(model.OtherID, false, "", delayClose: CloseSettlementDelay);
        }
        
        yield return GetWaitTimeModel(CloseSettlementDelay);
        
         //如果都满足
        if (SelfCostEnough && OtherCostEnough)
        {
            SetSettlementDamageRateValue(SelfID, true, SelfFinalDamageWelly);
            SetSettlementDamageRateValue(OtherID, true, OtherFinalDamageWelly);
            yield return GetWaitTimeModel(0.2f);
            if (!SelfInClasDamageRate && !OtherInClasDamageRate)//威力相同
            {
                SetSettlementDamageRateState(SelfID, false);
                SetSettlementDamageRateState(OtherID, false);
                
                //todo 双方UI被斩开表现
                //双方交锋后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                    model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
                //双方资源消耗表现 
                UnitResourceCost(SelfID, BattleRenderResourceCostReason.Clash);
                UnitResourceCost(OtherID, BattleRenderResourceCostReason.Clash);
                yield return GetWaitTimeModel(ResourceCostTime);
                //双方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                //双方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //双方行动结束
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
            else if (SelfInClasDamageRate)//我方胜利   //todo UI斩开还没写
            {
                SetSettlementDamageRateState(SelfID, true);
                SetSettlementDamageRateState(OtherID, false);
                yield return GetWaitTimeModel(0.2f);
                //双方交锋 后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                    model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
                //给对方添加破招buff表现
                if (LogicModel.GetSelfBeAddCounterBuff(OtherID))
                {
                    OtherRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                    yield return GetWaitTimeModel(AddBeCounterBuffTime);
                }
                //我方先行动
                if (LogicModel.GetSelfReleaseSkillSuccess(SelfID))//释放成功
                {
                    //我方消耗资源释放成功表现 
                    UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //我方去攻击表现 释放成功扳机表现
                    yield return PlayAttack(SelfRender, OtherRender);
                    //OtherRender.ShowDamage(model.GetSelfTruthDamage(SelfID), 0.3f);
                    //触发了破招 对方资源消耗表现
                    if (LogicModel.GetSelfBeTriggerCounterBuff(OtherID))
                    {
                        UnitResourceCost(OtherID, BattleRenderResourceCostReason.BeCounter);
                    }
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                    //触发了破招 对方行动后扳机表现
                    if (LogicModel.GetSelfBeTriggerCounterBuff(OtherID))
                    {
                        yield return WaitMomentShow(
                            model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                    }
                    
                    //我方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                    //我方行动结束表现
                    SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    //触发了破招 对方行动结束表现
                    if (LogicModel.GetSelfBeTriggerCounterBuff(OtherID))
                    { 
                        OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    }
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
                else//释放失败
                {
                    //我方消耗资源释放失败表现 
                    UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillFail);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                    //我方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                    //我方行动结束表现
                    SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
                //对方再行动
                if (LogicModel.GetSelfReleaseSkillSuccess(OtherID))//释放成功
                {
                    //对方消耗资源释放成功表现 
                    UnitResourceCost(OtherID, BattleRenderResourceCostReason.UseSkillSuccess);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //对方去攻击表现 释放成功扳机表现
                    yield return PlayAttack(OtherRender, SelfRender);
                    //SelfRender.ShowDamage(model.GetSelfTruthDamage(OtherID), 0.3f);
                    //我方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction));
                    //对方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                    //对方行动结束表现
                    OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
                else//释放失败
                {
                    //对方消耗资源释放失败表现 
                    UnitResourceCost(OtherID, BattleRenderResourceCostReason.UseSkillFail);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //我方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction));
                    //对方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                    //对方行动结束表现
                    OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
            }
            else//对方胜利
            {
                SetSettlementDamageRateState(SelfID, false);
                SetSettlementDamageRateState(OtherID, true);
                yield return GetWaitTimeModel(0.2f);
                //双方交锋 后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                    model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
                //给我方添加破招buff表现
                if (LogicModel.GetSelfBeAddCounterBuff(SelfID))
                {
                    SelfRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                    yield return GetWaitTimeModel(AddBeCounterBuffTime);
                }
                //对方先行动
                if (LogicModel.GetSelfReleaseSkillSuccess(OtherID))//释放成功
                {
                    //对方消耗资源释放成功表现
                    UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //对方去攻击表现 释放成功扳机表现
                    yield return PlayAttack(OtherRender, SelfRender);
                    //SelfRender.ShowDamage(model.GetSelfTruthDamage(OtherID), 0.3f);
                    //触发了破招 我方资源消耗表现
                    if (LogicModel.GetSelfBeTriggerCounterBuff(SelfID))
                    {
                        UnitResourceCost(SelfID, BattleRenderResourceCostReason.BeCounter);
                    }
                    //我方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction));
                    //触发了破招 我方行动后扳机表现
                    if (LogicModel.GetSelfBeTriggerCounterBuff(SelfID))
                    {
                        yield return WaitMomentShow(
                            model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                    }
                    
                    //对方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                    //对方行动结束表现
                    OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    //触发了破招 我方行动结束表现
                    if (LogicModel.GetSelfBeTriggerCounterBuff(SelfID))
                    {
                        SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    }
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
                else//释放失败
                {
                    //对方消耗资源释放失败表现 
                    UnitResourceCost(OtherID, BattleRenderResourceCostReason.UseSkillFail);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //我方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction));
                    //对方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                    //对方行动结束表现
                    OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
                //我方再行动
                if (LogicModel.GetSelfReleaseSkillSuccess(SelfID))//释放成功
                {
                    //我方消耗资源释放成功表现 
                    UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //去攻击表现 释放成功扳机表现
                    yield return PlayAttack(SelfRender, OtherRender);
                    //OtherRender.ShowDamage(model.GetSelfTruthDamage(SelfID), 0.3f);
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                    //我方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                    //我方行动结束表现
                    SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
                else//释放失败
                {
                    //我方消耗资源释放失败表现 
                    UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillFail);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                    //我方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                    //我方行动结束表现
                    SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
            }
        }
        else if (SelfCostEnough)//我方能释放
        {
            //todo 对方UI被斩开表现
            SetSettlementDamageRateState(SelfID, true);
            yield return GetWaitTimeModel(0.2f);
            //双方交锋后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
            //给对方添加破招buff表现
            if (LogicModel.GetSelfBeAddCounterBuff(OtherID))
            {
                OtherRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                yield return GetWaitTimeModel(AddBeCounterBuffTime);
            }
            if (LogicModel.GetSelfReleaseSkillSuccess(SelfID))//我方释放成功
            {
                //我方消耗资源释放成功表现 
                UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
                yield return GetWaitTimeModel(ResourceCostTime);
                //去攻击表现 释放成功扳机表现
                yield return PlayAttack(SelfRender, OtherRender);
                //OtherRender.ShowDamage(model.GetSelfTruthDamage(SelfID), 0.3f);
                //触发了破招 对方资源消耗表现
                if (LogicModel.GetSelfBeTriggerCounterBuff(OtherID))
                {
                    UnitResourceCost(OtherID, BattleRenderResourceCostReason.BeCounter);
                }
                //todo 我方结算UI消失表现
                //双方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                //触发了破招 对方行动后扳机表现
                if (LogicModel.GetSelfBeTriggerCounterBuff(OtherID))
                {
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                }
                //我方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                //我方行动结束表现
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                //触发了破招 对方行动结束表现
                if (LogicModel.GetSelfBeTriggerCounterBuff(OtherID))
                {
                    OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                }
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
            else//释放失败
            {
                //todo 我方结算UI被斩开表现
                //双方消耗资源释放失败表现 
                UnitResourceCost(SelfID, BattleRenderResourceCostReason.Clash);
                UnitResourceCost(OtherID, BattleRenderResourceCostReason.Clash);
                yield return GetWaitTimeModel(ResourceCostTime);
                //双方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                //双方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //双方行动结束表现
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
        }
        else if (OtherCostEnough)//对方能释放
        {
            //todo 我方UI被斩开表现
            SetSettlementDamageRateState(OtherID, true);
            yield return GetWaitTimeModel(0.2f);
            //双方交锋后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
            //给我方添加破招buff表现
            if (LogicModel.GetSelfBeAddCounterBuff(SelfID))
            {
                SelfRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                yield return GetWaitTimeModel(AddBeCounterBuffTime);
            }
            if (LogicModel.GetSelfReleaseSkillSuccess(OtherID))//对方释放成功
            {
                //对方消耗资源释放成功表现 
                UnitResourceCost(OtherID, BattleRenderResourceCostReason.UseSkillSuccess);
                yield return GetWaitTimeModel(ResourceCostTime);
                //对方去攻击表现 释放成功扳机表现
                yield return PlayAttack(OtherRender, SelfRender);
                //SelfRender.ShowDamage(model.GetSelfTruthDamage(OtherID), 0.3f);
                //触发了破招 我方资源消耗表现
                if (LogicModel.GetSelfBeTriggerCounterBuff(SelfID))
                {
                    UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
                    yield return GetWaitTimeModel(ResourceCostTime);
                }
                //todo 对方结算UI消失表现
                //双方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                //触发了破招 我方行动后扳机表现
                if (LogicModel.GetSelfBeTriggerCounterBuff(SelfID))
                {
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                }
                    
                //对方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //对方行动结束表现
                OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                //触发了破招 我方行动结束表现
                if (LogicModel.GetSelfBeTriggerCounterBuff(SelfID))
                {
                    SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                }
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
            else//释放失败
            {
                //todo 对方结算UI被斩开表现
                //双方消耗资源释放失败表现
                UnitResourceCost(SelfID, BattleRenderResourceCostReason.Clash);
                UnitResourceCost(OtherID, BattleRenderResourceCostReason.Clash);
                yield return GetWaitTimeModel(ResourceCostTime);
                //双方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
                //双方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //双方行动结束表现
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
        }
        else//都不能释放
        {
            //todo 双方UI被斩开表现
            //双方资源释放失败表现
            UnitResourceCost(SelfID, BattleRenderResourceCostReason.Clash);
            UnitResourceCost(OtherID, BattleRenderResourceCostReason.Clash);
            yield return GetWaitTimeModel(ResourceCostTime);
            //双方交锋后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
            //双方受到行动后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(SelfID, BattleMomentViewType.AfterUnderAction),
                model.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
            //双方行动后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(SelfID, BattleMomentViewType.AfterAction),
                model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
            //双方行动结束表现
            SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
            OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
            yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
        }
        
        SelfRender.MoveToBack(0.2f);
        OtherRender.MoveToBack(0.2f);
        yield return GetWaitTimeModel(0.2f);
    }
}

