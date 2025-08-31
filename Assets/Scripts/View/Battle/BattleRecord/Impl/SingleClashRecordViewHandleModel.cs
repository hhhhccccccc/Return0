using System;
using System.Collections;
using cfg;
using UnityEngine;
using Zenject;

public class SingleClashRecordViewHandleModel : RecordViewHandleModel<SingleClashRecordModel>
{
    private float SubjectInClasDamageRate;
    private float TargetInClasDamageRate;
    
    protected override void InitData()
    {
        base.InitData();
        SubjectInClasDamageRate = RecordModel.GetInClashSkillDamageRate(SubjectID);
        TargetInClasDamageRate = RecordModel.GetInClashSkillDamageRate(TargetID);
    }

    /// <summary>
    /// 行动结束表现 可能要做行动次数减一的表现
    /// </summary>
    protected override IEnumerator OnHandle()
    {
        var model = RecordModel;
        SetSettlementUI(model.SubjectID, true);
        SetSettlementUI(model.TargetID, true);
        yield return GetWaitTimeModel(0.2f);
        SetSettlementDamageRateValue(SubjectID, true, SubjectDamageRateDefault);
        SetSettlementDamageRateValue(TargetID, true, TargetDamageRateDefault);
        
        yield return WaitMomentShow(
            model.GetQueue(BattleMomentType.BeforeAction, SubjectID), 
            model.GetQueue(BattleMomentType.BeforeUnderAction, TargetID));
        
        yield return WaitMomentShow(
            model.GetQueue(BattleMomentType.BeforeClash, SubjectID), 
            model.GetQueue(BattleMomentType.BeforeClash, TargetID));

        var subjectCostEnough = model.CheckSubjectCostInClash;
        var targetCostEnough = model.CheckTargetCostInClash;
        
        if (!subjectCostEnough)
        {
            SetSettlementUI(model.SubjectID, false, "", delayClose: CloseSettlementDelay);
        }
        
        if (!targetCostEnough)
        {
            SetSettlementUI(model.TargetID, false, "", delayClose: CloseSettlementDelay);
        }
        
        yield return GetWaitTimeModel(CloseSettlementDelay);

        //如果都满足
        if (subjectCostEnough && targetCostEnough)
        {
            SetSettlementDamageRateValue(SubjectID, true, SubjectDamageRateFinal);
            SetSettlementDamageRateValue(TargetID, true, TargetDamageRateFinal);
            yield return GetWaitTimeModel(0.2f);
            if (Math.Abs(SubjectInClasDamageRate - TargetInClasDamageRate) <= 0.001f)//威力相同
            {
                SetSettlementDamageRateState(SubjectID, false);
                SetSettlementDamageRateState(TargetID, false);
                
                //todo 双方UI被斩开表现
                //双方交锋后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterClash, SubjectID), 
                    model.GetQueue(BattleMomentType.AfterClash, TargetID));
                //todo 双方资源消耗表现 
                //对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
                //双方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterAction, SubjectID),
                    model.GetQueue(BattleMomentType.AfterAction, TargetID));
                //双方行动结束
                SubjectRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                TargetRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
            else if (SubjectInClasDamageRate > TargetInClasDamageRate)//我方胜利
            {
                SetSettlementDamageRateState(SubjectID, true);
                SetSettlementDamageRateState(TargetID, false);
                yield return GetWaitTimeModel(0.2f);
                //todo 对方UI被斩开表现
                //双方交锋 后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterClash, SubjectID), 
                    model.GetQueue(BattleMomentType.AfterClash, TargetID));
                //添加破招buff表现
                if (model.Target_AddCounterBuff)
                {
                    TargetRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                    yield return GetWaitTimeModel(AddBeCounterBuffTime);
                }

                if (SubjectReleaseSkillSuccess)//释放成功
                {
                    //todo 我方消耗资源释放成功表现 
                    //todo 去攻击表现 释放成功扳机表现
                    //todo 触发了破招 对方资源消耗表现
                    if (model.Target_TriggerCounterBuff)
                    {
                        
                    }
                    //todo 我方结算UI消失表现
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
                    //触发了破招 对方行动后扳机表现
                    if (model.Target_TriggerCounterBuff)
                    {
                        yield return WaitMomentShow(
                            model.GetQueue(BattleMomentType.AfterAction, TargetID));
                    }
                    
                    //我方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(BattleMomentType.AfterAction, SubjectID));
                    //我方行动结束表现
                    SubjectRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    //触发了破招 对方行动结束表现
                    if (model.Target_TriggerCounterBuff)
                    { 
                        TargetRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    }
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
                else//释放失败
                {
                    //todo 我方消耗资源释放失败表现 
                    //todo 我方结算UI被斩开表现
                    //对方受到行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
                    //我方行动后扳机表现
                    yield return WaitMomentShow(
                        model.GetQueue(BattleMomentType.AfterAction, SubjectID));
                    //我方行动结束表现
                    SubjectRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                    yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
                }
            }
            else//对方胜利
            {
                SetSettlementDamageRateState(SubjectID, false);
                SetSettlementDamageRateState(TargetID, true);
                yield return GetWaitTimeModel(0.2f);
                //todo 我方UI被斩开表现
                //todo 对方UI被消失表现
                //双方交锋后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterClash, SubjectID), 
                    model.GetQueue(BattleMomentType.AfterClash, TargetID));
                //todo 我方消耗资源释放失败表现 
                //对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
                //我方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterAction, SubjectID));
                //我方行动结束表现
                SubjectRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
        }
        else if (subjectCostEnough)//我方能释放
        {
            //todo 对方UI被斩开表现
            SetSettlementDamageRateState(SubjectID, true);
            yield return GetWaitTimeModel(0.2f);
            //双方交锋后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(BattleMomentType.AfterClash, SubjectID), 
                model.GetQueue(BattleMomentType.AfterClash, TargetID));
            //添加破招buff表现
            if (model.Target_AddCounterBuff)
            {
                TargetRender.ShowAddBeCounterBuff(AddBeCounterBuffTime);
                yield return GetWaitTimeModel(AddBeCounterBuffTime);
            }
            if (SubjectReleaseSkillSuccess)//释放成功
            {
                //todo 我方消耗资源释放成功表现 
                //todo 去攻击表现 释放成功扳机表现
                //todo 触发了破招 对方资源消耗表现
                if (model.Target_TriggerCounterBuff)
                {
                        
                }
                //todo 我方结算UI消失表现
                //对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
                //触发了破招 对方行动后扳机表现
                if (model.Target_TriggerCounterBuff)
                {
                    yield return WaitMomentShow(
                        model.GetQueue(BattleMomentType.AfterAction, TargetID));
                }
                    
                //我方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterAction, SubjectID));
                //我方行动结束表现
                SubjectRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                //触发了破招 对方行动结束表现
                if (model.Target_TriggerCounterBuff)
                { 
                    TargetRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                }
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
            else//释放失败
            {
                //todo 我方结算UI被斩开表现
                //todo 我方消耗资源释放失败表现 
                //对方受到行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
                //我方行动后扳机表现
                yield return WaitMomentShow(
                    model.GetQueue(BattleMomentType.AfterAction, SubjectID));
                //我方行动结束表现
                SubjectRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
                yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
            }
        }
        else//都不能释放
        {
            //todo 双方UI被斩开表现
            //双方交锋后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(BattleMomentType.AfterClash, SubjectID), 
                model.GetQueue(BattleMomentType.AfterClash, TargetID));
            //对方受到行动后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
            //我方行动后扳机表现
            yield return WaitMomentShow(
                model.GetQueue(BattleMomentType.AfterAction, SubjectID));
            //我方行动结束表现
            SubjectRender.ShowReduceRoundTimes(1, ShowReduceRoundTimesTime);
            yield return GetWaitTimeModel(ShowReduceRoundTimesTime);
        }
    }
}

