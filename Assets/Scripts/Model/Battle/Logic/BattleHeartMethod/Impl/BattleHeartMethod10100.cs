using cfg;

//todo 表现
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
                    BattleBuffManager.AddBuff(target, GameConst.Battle.BuffLiShuai, Subject, GetParamInt(0));
                }
            }
        }
    }
}