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
                var clearBuffList = target.GetRandomBuffByType(BuffType.Abnormal, Config.ParamList[2].ToInt());
                var clearCount = clearBuffList.Count;
                foreach (var buff in clearBuffList)
                {
                    target.ClearBuff(buff.BuffID);
                }

                var poolID = Config.ParamList[1].ToInt();
                for (int i = 1; i <= clearCount; i++)
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
                            BattleBuffManager.AddBuff(target, newBuffID, target, newBuffLayerCount, null, MomentType);
                            break;
                        }
                    }
                }
            }
        }
    }
}