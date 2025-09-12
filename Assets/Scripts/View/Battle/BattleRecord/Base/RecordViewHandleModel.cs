using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public abstract class RecordViewHandleModel<T> : IRecordViewHandleModel, IModel
    where T : BattleRecordModel
{
    protected const float CloseSettlementDelay = 1.0f;
    protected const float ShowMomentDesTime = 0.5f;
    protected const float ShowReduceRoundTimesTime = 0.5f;
    protected const float AddBeCounterBuffTime = 0.5f;
    protected const float ResourceCostTime = 0.3f;
    
    protected T RecordModel;
    
    [Inject] protected BattleRenderManager BattleRenderManager;
    [Inject] protected IPoolManager PoolManager;
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] protected BattleManager BattleManager;
    [Inject] protected ConfigManager ConfigManager;
    [Inject] protected ILogManager LogManager;
    [Inject] protected IMessageManager MessageManager;

    //LogManager
    protected void Debug(string msg) => LogManager.Debug(msg);
    protected void Error(string msg) => LogManager.Error(msg);
    protected void Error(Exception e) => LogManager.Error(e);
    //MessageManager
    protected void DispatchMsg<TMsg>(TMsg msg) where TMsg : MessageModel => MessageManager.DispatchMsg(msg);
  
    //PoolManager
    protected TClass GetClass<TClass>() where TClass : class, new() => PoolManager.GetClass<TClass>();
    protected void RecycleClass<TClass>(TClass obj) where TClass : class => PoolManager.RecycleClass(obj);

    private List<WaitTimeModel> WaitTimeModelList = new();

    protected int SubjectID;
    protected int TargetID;
    protected BattleUnit SubjectLogic;
    protected BattleUnit TargetLogic;
    protected BattleUnitComponent SubjectRender;
    protected BattleUnitComponent TargetRender;
    protected float SubjectDamageRateDefault;
    protected float TargetDamageRateDefault;
    protected float SubjectDamageRateFinal;
    protected float TargetDamageRateFinal;
    protected bool SubjectReleaseSkillSuccess;
    protected bool TargetReleaseSkillSuccess;
    protected float SubjectGangQiCost;
    protected float TargetGangQiCost;
    protected float SubjectXuanQiCost;
    protected float TargetXuanQiCost;
    public IEnumerator Handle(BattleRecordModel recordModel, Action actEndCallback)
    {
        RecordModel = (T)recordModel;
        InitData();
        yield return OnHandle();
        RecycleWaitTimeModel();
        PoolManager.RecycleClass(RecordModel);
        PoolManager.RecycleClass(this);
        actEndCallback();
    }

    protected virtual void InitData()
    {
        SubjectID = RecordModel.SubjectID;
        TargetID = RecordModel.TargetID;
        SubjectLogic = BattleManager.GetUnit(SubjectID);
        TargetLogic = BattleManager.GetUnit(TargetID);
        SubjectRender = BattleRenderManager.GetUnit(SubjectID);
        TargetRender = BattleRenderManager.GetUnit(TargetID);
        SubjectDamageRateDefault = RecordModel.GetSkillDamageRateDefault(SubjectID);
        TargetDamageRateDefault = RecordModel.GetSkillDamageRateDefault(TargetID);
        SubjectDamageRateFinal = RecordModel.GetSkillDamageRateFinal(SubjectID);
        TargetDamageRateFinal = RecordModel.GetSkillDamageRateFinal(TargetID);
        SubjectReleaseSkillSuccess = RecordModel.GetReleaseSkillSuccess(SubjectID);
        TargetReleaseSkillSuccess = RecordModel.GetReleaseSkillSuccess(TargetID);
        SubjectGangQiCost = RecordModel.GetGangQiCost(SubjectID);
        TargetGangQiCost = RecordModel.GetGangQiCost(TargetID);
        SubjectXuanQiCost = RecordModel.GetXuanQiCost(SubjectID);
        TargetXuanQiCost = RecordModel.GetXuanQiCost(TargetID);
    }

    protected abstract IEnumerator OnHandle();

    protected WaitTimeModel GetWaitTimeModel(float waitTime)
    {
        var waitTimeModel = PoolManager.GetClass<WaitTimeModel>();
        waitTimeModel.Time = waitTime;
        WaitTimeModelList.Add(waitTimeModel);
        return waitTimeModel;
    }

    private void RecycleWaitTimeModel()
    {
        foreach (var waitTimeModel in WaitTimeModelList)
        {
            PoolManager.RecycleClass(waitTimeModel);
        }
        
        WaitTimeModelList.Clear();
    }

    protected void SetSettlementUI(int entityID, bool state, string aniName = "", float delayClose = 0)
    {
        /*var model = GetClass<BattleSetSettlementUIEventModel>();
        model.EntityID = entityID;
        model.State = state;
        model.AniName = aniName;
        model.DelayClose = delayClose;
        DispatchMsg(model);
        RecycleClass(model);*/
    }

    protected void ShowMomentDes(BattleMomentViewModel viewModel)
    {   
        var model = GetClass<BattleShowMomentRecordEventModel>();
        model.EntityID = viewModel.EntityID;
        model.BattleMomentType = viewModel.BattleMomentType;
        model.BattleSource = viewModel.BattleSource;
        model.ConfigID = viewModel.ConfigID;
        DispatchMsg(model);
        RecycleClass(model);
    }

    #region 等待扳机显示

    protected IEnumerator WaitMomentShow(Queue<BattleMomentViewModel> q1)
    {
        while (q1.Any())
        {   
            var viewModel = q1.Dequeue();
            ShowMomentDes(viewModel);
            yield return GetWaitTimeModel(ShowMomentDesTime);
        }
    }    
    
    protected IEnumerator WaitMomentShow(Queue<BattleMomentViewModel> q1, Queue<BattleMomentViewModel> q2)
    {
        while (q1.Any() || q2.Any())
        {   
            if (q1.Any())
            {
                var viewModel = q1.Dequeue();
                ShowMomentDes(viewModel);
            }

            if (q2.Any())
            {
                var viewModel = q2.Dequeue();
                ShowMomentDes(viewModel);
            }

            yield return GetWaitTimeModel(ShowMomentDesTime);
        }
    }
    
    protected IEnumerator WaitMomentShow(Queue<BattleMomentViewModel> q1, Queue<BattleMomentViewModel> q2, Queue<BattleMomentViewModel> q3, Queue<BattleMomentViewModel> q4)
    {
        while (q1.Any() || q2.Any() || q3.Any() || q4.Any())
        {   
            if (q1.Any())
            {
                var viewModel = q1.Dequeue();
                ShowMomentDes(viewModel);
            }

            if (q2.Any())
            {
                var viewModel = q2.Dequeue();
                ShowMomentDes(viewModel);
            }
            
            if (q3.Any())
            {
                var viewModel = q3.Dequeue();
                ShowMomentDes(viewModel);
            }
            
            if (q4.Any())
            {
                var viewModel = q4.Dequeue();
                ShowMomentDes(viewModel);
            }

            yield return GetWaitTimeModel(ShowMomentDesTime);
        }
    }
    
    #endregion

    #region 设置最终威力相关

    protected void SetSettlementDamageRateValue(int entityID, bool isSet, float damageRate)
    {
        var model = GetClass<BattleSetSettlementDamageRateValueEventModel>();
        model.EntityID = entityID;
        model.IsSet = isSet;
        model.DamageRate = damageRate;
        DispatchMsg(model);
        RecycleClass(model);
    }
    
    protected void SetSettlementDamageRateState(int entityID, bool isLight)
    {
        var model = GetClass<BattleSetSettlementDamageRateStateEventModel>();
        model.EntityID = entityID;
        model.IsLight = isLight;
        DispatchMsg(model);
        RecycleClass(model);
    }

    #endregion

    #region 英雄表现

    protected void UnitResourceCost(int entityID, BattleRenderResourceCostReason costReason)
    {
        var unit = entityID == SubjectID ? SubjectRender : TargetRender;
        unit.GangQiChanged(SubjectGangQiCost, ResourceCostTime);
        unit.XuanQiChanged(SubjectXuanQiCost, ResourceCostTime);
    }

    #endregion
    
}