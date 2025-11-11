using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_AddHasBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        var getTarList = GetUnitByParamID(Config.ParamList[0]);
        if (getTarList.Count > 0)
        {
            var buffType = Config.ParamList[2].ToInt();
            var buffCount = Config.ParamList[3].ToInt();
            var getTar = getTarList[0];
            var addTarList = GetUnitByParamID(Config.ParamList[1]);
            var buffList = getTar.GetRandomBuffByType((BuffType)buffType, buffCount);
            if (buffList.Count > 0)
            {
                foreach (var buff in buffList)
                {
                    foreach (var addTar in addTarList)
                    {
                        BattleBuffManager.AddBuff(addTar, buff.BuffID, getTar, buff.LayerCount, new List<float>(buff.ParamList), MomentType);
                    }
                }
            }
        }
    }
}