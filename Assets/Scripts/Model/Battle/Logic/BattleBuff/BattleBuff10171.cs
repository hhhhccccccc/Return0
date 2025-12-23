using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10171 : BattleBuffBase
{
    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        if (buffConfig.BuffType == (int)BuffType.Abnormal)
        {
            return false;
        }

        return true;
    }
}
