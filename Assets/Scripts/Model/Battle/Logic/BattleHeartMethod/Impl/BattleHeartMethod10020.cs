using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10020 : BattleHeartMethodBase
{
    private float SkillWelly => GetConfigParamFloat(0);
    public override float GetWellyRateEx(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) != SkillType.ArtKilling)
        {
            return 0;
        }
        
        if (Subject.RoundUsedSkillGuid.Count <= 0)
        {
            return GetConfigParamFloat(0);
        }

        if (!Subject.RoundUsedSkillGuid.Any(guid =>
            {
                var (skillID, variantID) = Util.UnCombSkillGuid(guid);
                return BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.ArtKilling;
            }))
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, SkillWelly);
            return SkillWelly;
        }

        return 0;
    }
}