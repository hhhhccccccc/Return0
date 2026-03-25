using System.Collections.Generic;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10004 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRecInt)
        {
            return GetParamFloat(0);
        }

        return 0;
    }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        subject.AddNotRecoverXuanQiNatural(1);
    }
}