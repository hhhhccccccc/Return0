using System.Collections.Generic;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10005 : BattleHeartMethodBase
{
    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRecInt)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }

    public override bool CheckCanRecoverNaturalQi(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.GangQi)
        {
            return false;
        }

        return true;
    }
}