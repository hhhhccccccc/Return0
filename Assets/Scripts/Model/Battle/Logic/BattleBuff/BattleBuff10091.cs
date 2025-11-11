using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10091 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.TempPowerSkillAddWellyRate)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
