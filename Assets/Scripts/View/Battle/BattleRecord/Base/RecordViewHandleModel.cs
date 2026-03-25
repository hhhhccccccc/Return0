using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public abstract class RecordViewHandleModel<T> : IRecordViewHandleModel, IModel, IRecycle
    where T : BattleRecordModel
{
    protected const float CloseSettlementDelay = 1.0f;
    protected const float ShowMomentDesTime = 0.5f;
    protected const float ShowReduceRoundTimesTime = 0.5f;
    protected const float AddBeCounterBuffTime = 0.5f;
    protected const float ResourceCostTime = 0.3f;
    
    protected T RecordModel { get; set; }
    
    [Inject] protected BattleRenderManager BattleRenderManager { get; set; }
    [Inject] protected IPoolManager PoolManager { get; set; }
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] protected BattleManager BattleManager { get; set; }
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] protected ILogManager LogManager { get; set; }
    [Inject] protected IMessageManager MessageManager { get; set; }

    //LogManager
    protected void Debug(string msg) => LogManager.D(msg);
    protected void Error(string msg) => LogManager.E(msg);
    protected void Error(Exception e) => LogManager.E(e);
    //MessageManager
    protected void DispatchMsg<TMsg>(TMsg msg) where TMsg : MessageModel => MessageManager.DispatchMsg(msg);
  
    //PoolManager
    protected TClass GetClass<TClass>() where TClass : class, new() => PoolManager.GetClass<TClass>();
    protected void RecycleClass<TClass>(TClass obj) where TClass : class => PoolManager.RecycleClass(obj);
    private List<WaitTimeModel> WaitTimeModelList = new();

    protected int SelfID { get; set; }
    protected int OtherID { get; set; }
    protected BattleUnit SelfLogic { get; set; }
    protected BattleUnit OtherLogic { get; set; }
    protected BattleUnitComponent SelfRender { get; set; }
    protected BattleUnitComponent OtherRender { get; set; }
    protected DamageParamModel LogicModel { get; set; }
    protected MomentViewParamModel ViewModel { get; set; }
    public IEnumerator Handle(BattleRecordModel recordModel, Action actEndCallback)
    {
        RecordModel = (T)recordModel;
        InitData();
        yield return OnHandle();
        RecycleWaitTimeModel();
        PoolManager.RecycleClass(this);
        actEndCallback();
    }

    protected virtual void InitData()
    {
        SelfID = RecordModel.SelfID;
        OtherID = RecordModel.OtherID;
        LogicModel = RecordModel.DamageParamModel;
        ViewModel = RecordModel.MomentViewParamModel;
        SelfLogic = BattleManager.GetUnit(SelfID);
        OtherLogic = BattleManager.GetUnit(OtherID);
        SelfRender = BattleRenderManager.GetUnit(SelfID);
        OtherRender = BattleRenderManager.GetUnit(OtherID);
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
        /*var unit = entityID == SelfID ? SelfRender : OtherRender;
        unit.GangQiChanged(SelfGangQiCost, ResourceCostTime);
        unit.XuanQiChanged(SelfXuanQiCost, ResourceCostTime);*/
    }

    #endregion
    
    public virtual void Recycle()
    {
        PoolManager.RecycleClass(RecordModel);
    }

    private List<float> AttackDamageList = new List<float>();
    private List<string> AttackAnimList = new List<string>();

    protected IEnumerator PlayAttack(BattleUnitComponent attack, BattleUnitComponent hit)
    {
        attack.MoveToTarget(OtherRender, 0.3f);
        yield return GetWaitTimeModel(0.3f);
        
        //伤害结算根据键类型分类
     
        var skillCost = LogicModel.GetSelfKeyCost(attack.EntityID);
        var damage = LogicModel.GetSelfAttackHpValue(attack.EntityID);
        var singleDamage = damage / skillCost.Count;
        
        for (int i = 0; i < skillCost.Count; i++)
        {
            var sameCount = 1;
            var index = i + 1;
            while (index < skillCost.Count && skillCost[i].KeyType == skillCost[index].KeyType)
            {
                index++;
                sameCount++;
            }
            AttackDamageList.Add(singleDamage * sameCount);
            AttackAnimList.Add($"Attack{sameCount}");
            i += sameCount - 1;
        }

        for (int i = 0; i < AttackDamageList.Count; i++)
        {
            attack.PlayAnim(AttackAnimList[i]);
            yield return GetWaitTimeModel(0.2f);
            hit.ShowDamage(AttackDamageList[i], 0.25f);
            yield return GetWaitTimeModel(0.25f);
        }
        
        yield return GetWaitTimeModel(0.25f);
        
        AttackDamageList.Clear();
        AttackAnimList.Clear();
    }
}