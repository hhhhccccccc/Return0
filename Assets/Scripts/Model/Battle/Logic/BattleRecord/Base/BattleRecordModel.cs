using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public abstract class BattleRecordModel : IModel, IRecycle
{
    [Inject] protected IPoolManager PoolManager { get; set; }
    [Inject] protected BattleRecordManager BattleRecordManager { get; set; }
    public virtual BattleClashType BattleClashType { get; private set; }

    //行动前判定 能否拉起单方面行动
    public bool CheckSelfCostPullFight { get; set; }
    public bool CheckSelfCostGenerateAction { get; set; }
    public int SelfID { get; set; }
    public int OtherID { get; set; }
    public DamageParamModel DamageParamModel { get; set; }
    public MomentViewParamModel MomentViewParamModel { get; set; }
    public void AddBattleMomentViewModel(BattleMomentViewType momentViewType, BattleMomentViewModel viewModel) =>
        MomentViewParamModel.AddBattleMomentViewModel(momentViewType, viewModel);
    public Queue<BattleMomentViewModel> GetQueue(int entityID, BattleMomentViewType momentViewType) =>
        MomentViewParamModel.GetQueue(entityID, momentViewType);

    public void Init(BattleClashType clashType, int selfID, int otherID)
    {
        SelfID = selfID;
        OtherID = otherID;
        DamageParamModel = PoolManager.GetClass<DamageParamModel>();
        DamageParamModel.BattleClashType = clashType;
        DamageParamModel.SetSelfID(selfID);
        DamageParamModel.SetOtherID(otherID);
        MomentViewParamModel = PoolManager.GetClass<MomentViewParamModel>();
        MomentViewParamModel.SelfViewModel.EntityID = selfID;
        MomentViewParamModel.OtherViewModel.EntityID = otherID;
    }
    
    public virtual void Recycle()
    {
        BattleClashType = BattleClashType.None;
        CheckSelfCostPullFight = false;
        CheckSelfCostGenerateAction = false;
        SelfID = 0;
        OtherID = 0;
        PoolManager.RecycleClass(DamageParamModel);
        PoolManager.RecycleClass(MomentViewParamModel);
    }
}
