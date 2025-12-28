using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10020 : BattleHeartMethodBase
{
    public override float GetSkillWellyRate(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) != SkillType.ArtKilling)
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
                return BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.ArtKilling;
            }))
        {
            return GetParamFloat(0);
        }

        return 0;
    }
}