using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10022 : BattleHeartMethodBase
{
    public override float GetSkillWellyRate(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) == SkillType.PowerKilling)
        {
            return GetParamFloat(0);
        }
        
        return 0;
    }
}