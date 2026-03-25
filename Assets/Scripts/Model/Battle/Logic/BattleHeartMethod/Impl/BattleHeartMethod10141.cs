using cfg;
//todo 表现
public class BattleHeartMethod10141 : BattleHeartMethodBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var skill = Subject.GetSkill();
            if (skill.GetSKillType == SkillType.PowerKilling || skill.GetSKillType == SkillType.ArtKilling)
            {
                var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
                if (target != null && !target.HasBuff(GameConst.Battle.Buff20071))
                {
                    BattleBuffManager.AddBuff(target, GameConst.Battle.Buff20071, Subject, GetParamInt(0));
                }
            }
        }
    }
}