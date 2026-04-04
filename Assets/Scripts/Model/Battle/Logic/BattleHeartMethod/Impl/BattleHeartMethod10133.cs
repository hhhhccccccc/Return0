using cfg;

//todo 表现
public class BattleHeartMethod10133 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(0));
        var buffCount = Subject.GetBuffCountByID(GameConst.Battle.BuffDuZhang);
        if (buffCount > GetConfigParamInt(1))
        {
            Subject.AddActionTimes(GetConfigParamInt(2));
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var skill = Subject.GetSkill();
        if (skill != null && paramModel is DamageParamModel model)
        {
            if (skill.GetSKillType == SkillType.PowerKilling || skill.GetSKillType == SkillType.ArtKilling)
            {
                var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
                BattleBuffManager.AddBuff(target, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(3));
            }

            if (skill.GetSKillType == SkillType.TechniqueImperialStyle)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, GetConfigParamInt(4));
            }
        }
    }
}