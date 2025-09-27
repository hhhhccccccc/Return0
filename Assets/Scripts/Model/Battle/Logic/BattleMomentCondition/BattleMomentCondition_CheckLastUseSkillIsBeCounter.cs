using Zenject;

public class BattleMomentCondition_CheckLastUseSkillIsBeCounter : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var skillID = Config.ParamList[1].ToInt();
            var relation = Config.ParamList[2].ToInt() == 1;
            var state = target.PreUseSkillDataManager.GetLastUseSkillState(skillID);

            if (relation)
            {
                return state == LastUseSkillState.BeCounter;
            }
            else
            {
                return state != LastUseSkillState.BeCounter;
            }
        }

        return false;
    }
}