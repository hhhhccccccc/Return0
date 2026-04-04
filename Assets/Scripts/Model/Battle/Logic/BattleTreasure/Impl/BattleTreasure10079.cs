using cfg;

//todo 表现
public class BattleTreasure10079 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        var hasCount = Subject.GetAllKeyCount();
        var limit = GetConfigParamInt(0);
        if (hasCount > limit)
        {
            var delta = hasCount - limit;
            for (int i = 0; i < delta; i++)
            {
                var result = ConfigHelper.RandomCommonPool(GetConfigParamInt(1));
                BattleBuffManager.AddBuff(Subject, result[0].ID, Subject, result[0].Num);
            }
        }
    }
}

