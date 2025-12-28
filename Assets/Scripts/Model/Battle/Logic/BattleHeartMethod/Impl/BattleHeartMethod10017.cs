using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10017 : BattleHeartMethodBase
{
    public override float GetSkillWellyRate(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) != SkillType.PowerKilling)
        {
            return 0;
        }
        
        if (Subject.RoundUsedSkillGuid.Count <= 0)
        {
            return GetParamFloat(0);
        }

        if (!Subject.RoundUsedSkillGuid.Any(guid =>
            {
                var (skillID, variantID) = Util.UnCombSkillGuid(guid);
                return BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.PowerKilling;
            }))
        {
            return GetParamFloat(0);
        }

        return 0;
    }
}