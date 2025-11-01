using System.Collections;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class SingleActionRecordViewHandleModel : RecordViewHandleModel<SingleActionRecordModel>
{
    protected override IEnumerator OnHandle() //todo 目前只展示了自己的结算UI  对方的还没显示 但是对方的受到行动前的扳机显示在哪里要做
    {
        var model = RecordModel;
        SetSettlementUI(model.SubjectID, true);
        if (model.CheckSubjectBeCounter) 
        {
            SetSettlementUI(model.SubjectID, false, "", delayClose: CloseSettlementDelay);
            yield return GetWaitTimeModel(CloseSettlementDelay);
            yield break;
        }

        if (!model.CheckSubjectCostPullFight)
        {
            SetSettlementUI(model.SubjectID, false, "", delayClose: CloseSettlementDelay);
            yield return GetWaitTimeModel(CloseSettlementDelay);
            yield break;
        }

        yield return WaitMomentShow(
            model.GetQueue(BattleMomentType.BeforeAction, SubjectID), 
            model.GetQueue(BattleMomentType.BeforeUnderAction, TargetID));
        
        if (!model.CheckSubjectCostGenerateAction)
        {
            SetSettlementUI(model.SubjectID, false, "", delayClose: CloseSettlementDelay);
            UnitResourceCost(SubjectID, BattleRenderResourceCostReason.UseSkillFail);
            yield return GetWaitTimeModel(CloseSettlementDelay);
            yield break;
        }
        
        //随便等一下下
        yield return GetWaitTimeModel(0.2f);
        UnitResourceCost(SubjectID, BattleRenderResourceCostReason.UseSkillSuccess);
        yield return GetWaitTimeModel(0.3f);
        SubjectRender.MoveToTarget(TargetRender, 0.3f);
        yield return GetWaitTimeModel(0.3f);
        SubjectRender.PlayAnim("Attack1");
        yield return GetWaitTimeModel(0.25f);
        TargetRender.ShowDamage(model.GetTruthDamage(SubjectID), 0.3f);
        yield return GetWaitTimeModel(0.3f);
        
        yield return WaitMomentShow(
            model.GetQueue(BattleMomentType.AfterAction, SubjectID), 
            model.GetQueue(BattleMomentType.AfterUnderAction, TargetID));
        
        yield return GetWaitTimeModel(0.4f);
        
        SetSettlementUI(model.SubjectID, false, "", 0);
        yield return GetWaitTimeModel(0.2f);
        SubjectRender.MoveToBack(0.2f);
        yield return GetWaitTimeModel(0.2f);
    }
}

