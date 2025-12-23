using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10094 : BattleHeartMethodBase
{
    private List<int> BanBuffIDList = new()
    {
        GameConst.Battle.Buff20151,
        GameConst.Battle.Buff20161,
        GameConst.Battle.Buff20181,
        GameConst.Battle.Buff20191,
        GameConst.Battle.Buff20201,
        GameConst.Battle.Buff20211,
    };
    
    private bool CanAdd { get; set; }
    private int RoundDelta { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanAdd = true;
    }

    public override void RoundEnd()
    {
        if (RoundDelta > 0)
        {
            RoundDelta--;
            if (RoundDelta == 0)
            {
                CanAdd = true;
            }
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
            CanAdd = false;
            RoundDelta = GetParamInt(1);
        }
    }

    public override void RoundStart()
    {
        Subject.AddActionTimes(GetParamInt(0));
    }

    public override void Recycle()
    {
        CanAdd = false;
        base.Recycle();
    }
}