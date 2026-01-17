using System;
using System.Collections;
using cfg;
using UnityEngine;
using Zenject;

public class SingleClashRecordViewHandleModel : RecordViewHandleModel<SingleClashRecordModel>
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
        SelfDefaultDamageWelly = LogicModel.GetSelfDefaultDamageWelly(SelfID);
        OtherDefaultDamageWelly = LogicModel.GetSelfDefaultDamageWelly(OtherID);
        SelfFinalDamageWelly = LogicModel.GetSelfFinalDamageWelly(SelfID);
        OtherFinalDamageWelly = LogicModel.GetSelfFinalDamageWelly(OtherID);
        SelfInClasDamageRate = LogicModel.GetSelfClashState(SelfID);
        OtherInClasDamageRate = LogicModel.GetSelfClashState(OtherID);
    }

    /// <summary>
    /// 行动结束表现 可能要做行动次数减一的表现
    /// </summary>
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
            model.GetQueue(OtherID, BattleMomentViewType.BeforeAction));
        
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
            if (!SelfInClasDamageRate && !OtherInClasDamageRate)//双方都失败
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
                /*//对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));*/
                //双方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction),
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //双方行动结束
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
            else if (SelfInClasDamageRate)//我方胜利
            {
                SetSettlementDamageRateState(SelfID, true);
                SetSettlementDamageRateState(OtherID, false);
                yield return GetWaitTimeModel(0.2f);
                //todo 对方UI被斩开表现
                //双方交锋 后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                    model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
                //添加破招buff表现
                /*if (model.OtherAddCounterBuff)
                {
                    OtherRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                    yield return GetWaitTimeModel(AddBeCounterBuffTime);
                }*/

                if (LogicModel.GetSelfReleaseSkillSuccess(SelfID))//释放成功
                {
                    //我方释放成功消耗资源表现 
                    UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
                    yield return GetWaitTimeModel(ResourceCostTime);
                    //去攻击表现 释放成功扳机表现
                    SelfRender.MoveToTarget(OtherRender, 0.3f);
                    yield return GetWaitTimeModel(0.3f);
                    SelfRender.PlayAnim("Attack1");
                    yield return GetWaitTimeModel(0.25f);
                    //OtherRender.ShowDamage(model.GetSelfTruthDamage(SelfID), 0.3f);
                    //触发了破招 对方资源消耗表现
                    if (LogicModel.GetSelfBeTriggerCounterBuff(OtherID))
                    { 
                        UnitResourceCost(OtherID, BattleRenderResourceCostReason.BeCounter);
                    }
                    //todo 我方结算UI消失表现
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
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
                    //todo 我方结算UI被斩开表现
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                    //我方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                    //我方行动结束表现
                    SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
            }
            else//对方胜利
            {
                SetSettlementDamageRateState(SelfID, false);
                SetSettlementDamageRateState(OtherID, true);
                yield return GetWaitTimeModel(0.2f);
                //todo 我方UI被斩开表现
                //todo 对方UI被消失表现
                //双方交锋后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                    model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
                //我方消耗资源释放失败表现 
                UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillFail);
                yield return GetWaitTimeModel(ResourceCostTime);
                //对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //我方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                //我方行动结束表现
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
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
            //添加破招buff表现
            /*if (model.OtherAddCounterBuff)
            {
                OtherRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                yield return GetWaitTimeModel(AddBeCounterBuffTime);
            }*/
            if (LogicModel.GetSelfReleaseSkillSuccess(SelfID))//释放成功
            {
                //我方消耗资源释放成功表现 
                UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
                yield return GetWaitTimeModel(ResourceCostTime);
                //去攻击表现 释放成功扳机表现
                SelfRender.MoveToTarget(OtherRender, 0.3f);
                yield return GetWaitTimeModel(0.3f);
                SelfRender.PlayAnim("Attack1");
                yield return GetWaitTimeModel(0.25f);
                //OtherRender.ShowDamage(model.GetSelfTruthDamage(SelfID), 0.3f);
                //触发了破招 对方资源消耗表现
                /*if (model.OtherTriggerCounterBuff)
                {
                    UnitResourceCost(OtherID, BattleRenderResourceCostReason.BeCounter);
                }*/
                //todo 我方结算UI消失表现
                //对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //触发了破招 对方行动后扳机表现
                /*if (model.OtherTriggerCounterBuff)
                {
                    yield return WaitMomentShow(
                        model.GetQueue(BattleMomentViewType.AfterAction, OtherID));
                }*/
                    
                //我方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                //我方行动结束表现
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                //触发了破招 对方行动结束表现
                /*if (model.OtherTriggerCounterBuff)
                { 
                    OtherRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                }*/
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
            else//释放失败
            {
                //todo 我方结算UI被斩开表现
                //我方消耗资源释放失败表现 
                UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillFail);
                yield return GetWaitTimeModel(ResourceCostTime);
                //对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
                //我方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
                //我方行动结束表现
                SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
        }
        else//都不能释放
        {
            //todo 双方UI被斩开表现
            //双方交锋后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(SelfID, BattleMomentViewType.AfterClash), 
                model.GetQueue(OtherID, BattleMomentViewType.AfterClash));
            //对方受到行动后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(OtherID, BattleMomentViewType.AfterAction));
            //我方行动后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(SelfID, BattleMomentViewType.AfterAction));
            //我方行动结束表现
            SelfRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
            yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
        }
        
        SelfRender.MoveToBack(0.2f);
        yield return GetWaitTimeModel(0.2f);
    }
}

