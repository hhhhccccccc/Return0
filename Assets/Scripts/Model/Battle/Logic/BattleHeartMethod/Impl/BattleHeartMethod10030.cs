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
        if (Subject.GetProperty(BattlePropertyType.GangQi) >= GetConfigParamFloat(0))
        {
            var addCount = Util.GetRandomInt(GetConfigParamInt(1), GetConfigParamInt(2) + 1);
            DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, addCount, null, BattleMomentType.DoDesitionAction);
        }
    }
}