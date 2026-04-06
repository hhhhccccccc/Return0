using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20281 : BattleBuffBase
{
    //无法获得增益状态
    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        if (buffConfig.BuffType == (int)BuffType.Gain)
        {
            return false;
        }

        return true;
    }
}
