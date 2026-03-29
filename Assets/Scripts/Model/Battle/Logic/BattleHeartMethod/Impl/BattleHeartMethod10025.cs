using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10025 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        var buffID = Util.GetRandomBool() ? GameConst.Battle.BuffXunSu : GameConst.Battle.BuffHuanSu;
        BattleBuffManager.AddBuff(Subject, buffID, Subject, GetParamInt(0));
    }

    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.SpeedInt)
        {
            return GetParamFloat(1) + GetParamFloat(2) * Subject.Gr;
        }

        return 0;
    }
}