using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10101 : BattleBuffBase
{
    /// <summary>
    /// 每层使术杀式威力增加5百分比
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.TempArtSkillWellyRateEx)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
