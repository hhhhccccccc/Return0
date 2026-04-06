using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10091 : BattleBuffBase
{
    /// <summary>
    /// 每层使武杀式威力增加5百分比
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.TempPowerSkillWellyRateEx)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
