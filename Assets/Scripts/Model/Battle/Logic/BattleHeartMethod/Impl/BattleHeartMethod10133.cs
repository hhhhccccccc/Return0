using cfg;

public class BattleHeartMethod10133 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(0), null, BattleMomentType.RoundStart);
        var buffCount = Subject.GetBuffCountByID(GameConst.Battle.BuffDuZhang);
        if (buffCount > GetConfigParamInt(1))
        {
            DoAddActionTimes(Subject, GetConfigParamInt(2));
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var skill = Subject.GetSkill();
        if (skill != null && paramModel is DamageParamModel model)
        {
            if (skill.GetSKillType == SkillType.PowerKilling || skill.GetSKillType == SkillType.ArtKilling || skill.GetSKillType == SkillType.SpellFormula)
            {
                var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
                DoAddBuff(target, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(3), null, BattleMomentType.ReleaseSkillAction);
            }

            if (skill.GetSKillType == SkillType.TechniqueImperialStyle)
            {
                DoAddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(4), null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }
}