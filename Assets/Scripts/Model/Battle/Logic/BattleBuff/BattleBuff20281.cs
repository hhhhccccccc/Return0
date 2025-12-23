using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20281 : BattleBuffBase
{
    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        if (buffConfig.BuffType == (int)BuffType.Gain)
        {
            return false;
        }

        return true;
    }
}
