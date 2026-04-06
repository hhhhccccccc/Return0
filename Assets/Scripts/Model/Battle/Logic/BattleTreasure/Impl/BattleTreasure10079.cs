using cfg;

public class BattleTreasure10079 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        var hasCount = Subject.GetAllKeyCount();
        var limit = GetConfigParamInt(0);
        var delta = hasCount - limit;
        if (delta > 0)
        {
            for (int i = 0; i < delta; i++)
            {
                var result = ConfigHelper.RandomCommonPool(GetConfigParamInt(1));
                DoAddBuff(Subject, result[0].ID, Subject, result[0].Num, null, BattleMomentType.RoundStart);
            }
        }
    }
}

