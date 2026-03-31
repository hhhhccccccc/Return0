using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30101 : BattleBuffBase
{
    
    protected override float OnGetWellyRateEx(int skillGuid)
    {
        var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
        if (skillID == GameConst.Battle.SkillFuXiaoJian)
        {
            return Config.ParamEx[0];
        }

        return 0;
    }

    protected override float OnGetAddWellyEffect(int skillGuid)
    {
        var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
        if (skillID == GameConst.Battle.SkillFuXiaoJian)
        {
            return Config.ParamEx[1];
        }

        return 0;
    }
}
