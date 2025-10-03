using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ConvertBuffAbnormalToGain : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private ConfigHelper ConfigHelper { get; set; }

    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var checkID = Config.ParamList[1].ToInt();
                var checkBuff = target.GetBuff(checkID);
                if (checkBuff == null || checkBuff.LayerCount <= 0)
                    return;

                var times = checkBuff.LayerCount * Config.ParamList[2].ToInt();
                var poolID = Config.ParamList[3].ToInt();
                for (int i = 1; i <= times; i++)
                {
                    var randomCount = GameConst.Battle.MaxRandomCount;
                    while (randomCount > 0)
                    {
                        randomCount--;
                        var poolResult = ConfigHelper.RandomCommonPool(poolID);
                        var newBuffID = poolResult[0].ID;
                        var newBuffLayerCount = poolResult[0].Num;
                        var originBuff = target.GetBuff(newBuffID);
                        if (originBuff == null || !originBuff.IsMaxLayer())
                        {
                            BattleBuffManager.AddBuff(target, newBuffID, target, newBuffLayerCount);
                            break;
                        }
                    }
                }
            }
        }
    }
}