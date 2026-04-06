using cfg;

public class BattleHeartMethod10100 : BattleHeartMethodBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.BattleClashType == BattleClashType.SingleAction)
            {
                var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
                if (!target.HasBuff(GameConst.Battle.BuffLiShuai))
                {
                    DoAddBuff(target, GameConst.Battle.BuffLiShuai, Subject, GetConfigParamInt(0), null, BattleMomentType.ReleaseSkillAction);
                }
            }
        }
    }
}