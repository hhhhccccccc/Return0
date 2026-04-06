using cfg;
using System.Linq;
public class BattleTreasure10224 : BattleTreasureBase
{
    protected override bool OnCheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType)
    {
        if (Subject.GetBuffList().Count(buff => buff.Config.BuffType == (int)BuffType.Abnormal) >= GetConfigParamInt(0))
        {
            return false;
        }

        return true;
    }
}


