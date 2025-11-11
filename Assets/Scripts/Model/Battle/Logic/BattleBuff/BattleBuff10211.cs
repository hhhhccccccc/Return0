using System;
using cfg;

public class BattleBuff10211 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        base.OnRoundStart();
        //扣除所有异常状态1层
        var abnormalBuffList = Subject.GetRandomBuffByType(BuffType.Abnormal);
        foreach (var buff in abnormalBuffList)
        {
            Subject.ReduceBuffLayerCount(buff.BuffID, 1);
        }

        //恢复75 + 15 * GR 护体
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject,
            (Config.ParamEx[1] + Subject.Gr * Config.ParamEx[2]).ToInt());
    }

    public override void ClearLayerCount()
    {
        var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
        ReduceLayerCount(reduceCount);
    }
}
