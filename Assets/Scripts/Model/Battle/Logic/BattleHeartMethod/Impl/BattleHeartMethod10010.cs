using System.Collections.Generic;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10010 : BattleHeartMethodBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffYuShouJiaShi, Subject, GetParamInt(0));
    }
}