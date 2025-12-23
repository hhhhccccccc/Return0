using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10125 : BattleHeartMethodBase
{
    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        //心法10125 回绝 缓速和失衡
        if (Subject.HasBuff(GameConst.Battle.Buff30391) && (buffID == GameConst.Battle.Buff20011 || buffID == GameConst.Battle.Buff20021))
        {
            return false;
        }

        return true;
    }
}