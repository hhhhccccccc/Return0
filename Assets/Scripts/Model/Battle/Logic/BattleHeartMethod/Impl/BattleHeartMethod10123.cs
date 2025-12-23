using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10123 : BattleHeartMethodBase
{
    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        //心法10123 回绝 力衰和武衰
        if (Subject.HasBuff(GameConst.Battle.Buff30371) && (buffID == GameConst.Battle.Buff20111 || buffID == GameConst.Battle.Buff20131))
        {
            return false;
        }

        return true;
    }
}