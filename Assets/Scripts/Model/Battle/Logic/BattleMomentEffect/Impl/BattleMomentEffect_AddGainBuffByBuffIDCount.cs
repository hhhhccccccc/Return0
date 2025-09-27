using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_AddGainBuffByBuffIDCount : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private ConfigHelper ConfigHelper { get; set; }
    
    protected override void OnEffect()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        var buffList = target.GetRandomBuffByType(BuffType.Abnormal, Config.ParamList[2].ToInt());
        foreach (var buff in buffList)
        {
            target.ClearBuff(buff.BuffID);
            var poolID = Config.ParamList[1].ToInt();
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