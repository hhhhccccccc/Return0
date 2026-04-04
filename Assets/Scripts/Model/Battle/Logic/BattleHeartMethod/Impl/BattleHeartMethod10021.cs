using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10021 : BattleHeartMethodBase
{
    private float SkillWelly => GetConfigParamFloat(0);
    public override float GetWellyRateEx(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) == SkillType.PowerKilling)
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, SkillWelly);
            return SkillWelly;
        }
        
        return 0;
    }
}