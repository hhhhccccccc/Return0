using System.Collections;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class SingleActionRecordViewHandleModel : RecordViewHandleModel<SingleActionRecordModel>
{
    protected override IEnumerator OnHandle() //todo 目前只展示了自己的结算UI  对方的还没显示 但是对方的受到行动前的扳机显示在哪里要做
    {
        //var model = RecordModel;
        
        SetSettlementUI(SelfID, true);
  
        if (!RecordModel.CheckSelfCostPullFight)
        {
            SetSettlementUI(SelfID, false, "", delayClose: CloseSettlementDelay);
            yield return GetWaitTimeModel(CloseSettlementDelay);
            yield break;
        }

        yield return WaitMomentShow(
            RecordModel.GetQueue(SelfID, BattleMomentViewType.BeforeAction), 
            RecordModel.GetQueue(OtherID, BattleMomentViewType.BeforeUnderAction));
        
        if (!RecordModel.CheckSelfCostGenerateAction)
        {
            SetSettlementUI(SelfID, false, "", delayClose: CloseSettlementDelay);
            UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillFail);
            yield return GetWaitTimeModel(CloseSettlementDelay);
            yield break;
        }
        
        //随便等一下下
        yield return GetWaitTimeModel(0.2f);
        UnitResourceCost(SelfID, BattleRenderResourceCostReason.UseSkillSuccess);
        yield return GetWaitTimeModel(0.3f);
        yield return PlayAttack(SelfRender, OtherRender);
        //OtherRender.ShowDamage(model.GetSelfTruthDamage(SelfID), 0.3f);
        yield return GetWaitTimeModel(0.3f);
        
        yield return WaitMomentShow(
            RecordModel.GetQueue(SelfID, BattleMomentViewType.AfterAction), 
            RecordModel.GetQueue(OtherID, BattleMomentViewType.AfterUnderAction));
        
        yield return GetWaitTimeModel(0.4f);
        
        SetSettlementUI(SelfID, false, "", 0);
        yield return GetWaitTimeModel(0.2f);
        SelfRender.MoveToBack(0.2f);
        yield return GetWaitTimeModel(0.2f);
    }
}

