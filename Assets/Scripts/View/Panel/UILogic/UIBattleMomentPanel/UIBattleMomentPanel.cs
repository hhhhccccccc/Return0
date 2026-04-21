using System.Collections.Generic;
using cfg;
using Zenject;

public partial class UIBattleMomentPanel
{
    [Inject] private BattleManager BattleManager { get; set; }
    private List<BattleMomentDesItem> SelfItemList = new();
    private List<BattleMomentDesItem> OtherItemList = new();
    
    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        Register<BattleShowMomentRecordEventModel>(OnShowBattleMomentRecord);
    }

    private void OnShowBattleMomentRecord(BattleShowMomentRecordEventModel model)
    {
        if (model.BattleMomentType == BattleMomentType.BeforeAction 
            || model.BattleMomentType == BattleMomentType.BeforeUnderAction
            || model.BattleMomentType == BattleMomentType.BeforeClash)
        {
            return;
        }

        if (BattleManager.CheckIsSelfUnit(model.EntityID))
        {
            var item = CreateItemByType<BattleMomentDesItem>(TfSelfMomentContent.transform);
            item.ShowText($"EntityID : {model.EntityID}, MomentType : {model.BattleMomentType}, BattleSource : {model.BattleSource}, ConfigID : {model.ConfigID}");
            SelfItemList.Add(item); 
        }
        else
        {
            var item = CreateItemByType<BattleMomentDesItem>(TfOtherMomentContent.transform);
            item.ShowText($"EntityID : {model.EntityID}, MomentType : {model.BattleMomentType}, BattleSource : {model.BattleSource}, ConfigID : {model.ConfigID}");
            OtherItemList.Add(item); 
        }
    }
}
