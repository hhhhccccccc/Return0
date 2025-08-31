using cfg;
using UnityEngine;
using Zenject;

public partial class UIBattleSettlementPanel
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    
    private BattleSettlementItem SelfSettlementItem { get; set; }
    private BattleSettlementItem OtherSettlementItem { get; set; }
    
    public override void OnShow()
    {
        base.OnShow();
        SelfSettlementItem = CreateUIComponentByType<BattleSettlementItem>(Content.transform);
        SelfSettlementItem.SetActive(false);
        OtherSettlementItem = CreateUIComponentByType<BattleSettlementItem>(Content.transform);
        OtherSettlementItem.SetActive(false);
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        Register<BattleSetSettlementUIEventModel>(OnBattleShowSettlement);
        Register<BattleShowMomentRecordEventModel>(OnBattleShowMomentRecord);
    }

    private void OnBattleShowSettlement(BattleSetSettlementUIEventModel model)
    {
        var logicUnit = BattleManager.GetUnit(model.EntityID);
        var renderUnit = BattleRenderManager.GetUnit(model.EntityID);
        if (logicUnit != null)
        {
            var item = logicUnit.IsSelf ? SelfSettlementItem : OtherSettlementItem;
            if (model.State)
            {
                item.gameObject.SetActive(true);
                var pos = UIManager.ConvertWorldToUIPosition(renderUnit.transform.position,
                    GetComponent<RectTransform>());
                item.transform.localPosition = pos;
            }
            else
            {
                if (model.DelayClose > 0)
                {
                    BattleRenderManager.DelayCall(() =>
                    {
                        item.gameObject.SetActive(false);
                    }, model.DelayClose);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
            
            if (!string.IsNullOrEmpty(model.AniName))
            {
                item.PlayAnim(model.AniName);
            }
        }
    }

    private void OnBattleShowMomentRecord(BattleShowMomentRecordEventModel model)
    {
        if (model.BattleMomentType != BattleMomentType.BeforeAction 
            && model.BattleMomentType != BattleMomentType.BeforeUnderAction
            && model.BattleMomentType != BattleMomentType.BeforeClash)
        {
            return;
        }

        var item = BattleManager.CheckIsSelfUnit(model.EntityID) ? SelfSettlementItem : OtherSettlementItem;
        item.ShowMoment(model.EntityID, model.BattleMomentType, model.BattleSource, model.ConfigID);
    }
}
