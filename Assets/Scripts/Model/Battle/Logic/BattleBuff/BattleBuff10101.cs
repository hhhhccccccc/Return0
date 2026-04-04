using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10101 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.TempArtSkillWellyRateEx)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
