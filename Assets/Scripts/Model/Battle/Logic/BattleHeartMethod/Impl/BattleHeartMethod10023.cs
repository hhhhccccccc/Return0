using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10023 : BattleHeartMethodBase
{
    public override float GetSkillWellyRate(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) == SkillType.ArtKilling)
        {
            return GetParamFloat(0);
        }
        
        return 0;
    }
}