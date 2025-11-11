using Zenject;

public class BattleMomentCondition_CheckLastUseSkillIsBeCounter : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var skill = target.GetSkill();
            if (skill == null)
            {
                return false;
            }
            var relation = Config.ParamList[1].ToInt() == 1;
            var state = target.PreUseSkillDataManager.GetLastUseSkillState(skill.SkillGuid);

            if (relation)
            {
                return state == LastUseSkillState.BeCounter;
            }

            return state != LastUseSkillState.BeCounter;
        }

        return false;
    }
}