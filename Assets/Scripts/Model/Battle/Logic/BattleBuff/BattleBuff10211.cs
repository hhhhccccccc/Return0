using System;
using cfg;

public class BattleBuff10211 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        //扣除所有异常状态1层
        var abnormalBuffList = Subject.GetRandomBuffByType(BuffType.Abnormal);
        foreach (var buff in abnormalBuffList)
        {
            DoReduceBuffLayerCount(Subject, buff.BuffID, 1);
        }

        DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject,
            (GetConfigParamInt(1) + GetConfigParamInt(2) * Subject.Gr), null, BattleMomentType.RoundStart);
    }

    public override int ClearLayerCount()
    {
        var reduceCount = Math.Min(GetConfigParamInt(0), LayerCount);
        return ReduceLayerCount(reduceCount);
    }
}
