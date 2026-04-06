using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10171 : BattleBuffBase
{
    /// <summary>
    /// 无法被施加异常状态
    /// </summary>
    /// <param name="buffID"></param>
    /// <param name="addCount"></param>
    /// <param name="spellCasterID"></param>
    /// <param name="momentType"></param>
    /// <returns></returns>
    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        if (buffConfig.BuffType == (int)BuffType.Abnormal)
        {
            return false;
        }

        return true;
    }
}
