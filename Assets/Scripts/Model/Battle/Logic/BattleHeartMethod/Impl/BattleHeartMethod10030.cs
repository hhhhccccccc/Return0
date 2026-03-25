using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10030 : BattleHeartMethodBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        if (Subject.GetProperty(BattlePropertyType.GangQi) >= GetParamFloat(0))
        {
            var addCount = Util.GetRandomInt(GetParamInt(1), GetParamInt(2) + 1);
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10091, Subject, addCount);
        }
    }
}