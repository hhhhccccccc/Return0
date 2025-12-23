using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10124 : BattleHeartMethodBase
{
    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        //心法10124 回绝 技衰和术衰
        if (Subject.HasBuff(GameConst.Battle.Buff30381) && (buffID == GameConst.Battle.Buff20121 || buffID == GameConst.Battle.Buff20141))
        {
            return false;
        }

        return true;
    }
}