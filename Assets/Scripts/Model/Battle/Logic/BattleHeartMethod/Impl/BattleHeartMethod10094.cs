using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10094 : BattleHeartMethodBase
{
    private int Times => GetParamInt(0);
    private List<int> BanBuffIDList = new()
    {
        GameConst.Battle.Buff20151,
        GameConst.Battle.Buff20161,
        GameConst.Battle.Buff20181,
        GameConst.Battle.Buff20191,
        GameConst.Battle.Buff20201,
        GameConst.Battle.Buff20211,
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
            addCount = GetParamInt(2);
            return CanAdd;
        }

        return true;
    }

    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (BanBuffIDList.Contains(buffID))
        {
            RoundDelta = GetParamInt(1);
        }
    }

    public override void RoundStart()
    {
        Subject.AddActionTimes(Times);
        EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
    }
}