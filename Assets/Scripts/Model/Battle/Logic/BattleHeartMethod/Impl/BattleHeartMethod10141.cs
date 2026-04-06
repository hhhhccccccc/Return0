using cfg;

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
                if (target != null && !target.HasBuff(GameConst.Battle.BuffPoZhan))
                {
                    DoAddBuff(target, GameConst.Battle.BuffPoZhan, Subject, GetConfigParamInt(0), null, BattleMomentType.ReleaseSkillAction);
                }
            }
        }
    }
}