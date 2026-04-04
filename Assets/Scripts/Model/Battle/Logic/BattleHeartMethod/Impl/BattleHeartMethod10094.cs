using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10094 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(0);
    private List<int> BanBuffIDList = new()
    {
        GameConst.Battle.BuffJiangYing,
        GameConst.Battle.BuffGuoJin,
        GameConst.Battle.BuffWuShiJin,
        GameConst.Battle.BuffShuShiJin,
        GameConst.Battle.BuffJiShiJin,
        GameConst.Battle.BuffFaShiJin,
    };

    private bool CanAdd => RoundDelta <= 0;
    private int RoundDelta { get; set; }
    public override void RoundEnd()
    {
        if (RoundDelta > 0)
        {
            RoundDelta--;
        }
        base.RoundEnd();
    }

    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        if (BanBuffIDList.Contains(buffID))
        {
            addCount = GetConfigParamInt(2);
            return CanAdd;
        }

        return true;
    }

    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (BanBuffIDList.Contains(buffID))
        {
            RoundDelta = GetConfigParamInt(1);
        }
    }

    public override void RoundStart()
    {
        Subject.AddActionTimes(Times);
        EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
    }
}