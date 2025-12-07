using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10005 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.XuanQiRecInt)
        {
            return GetParamFloat(0);
        }

        return 0;
    }
    
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        subject.AddNotRecoverGangQiNatural(1);
    }
}