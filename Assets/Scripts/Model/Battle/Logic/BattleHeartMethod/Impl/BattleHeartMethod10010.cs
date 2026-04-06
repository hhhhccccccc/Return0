using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10010 : BattleHeartMethodBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffYuShouJiaShi, Subject, GetConfigParamInt(0), null,BattleMomentType.AfterAction);
    }
}