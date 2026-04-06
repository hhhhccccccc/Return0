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
    
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        subject.AddNotRecoverGangQiNatural(1);
    }
}