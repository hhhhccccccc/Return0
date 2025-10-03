using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_AddBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var addSpellList = GetUnitByParamID(Config.ParamList[0]);
        var addTarList = GetUnitByParamID(Config.ParamList[1]);
        if (addSpellList.Count > 0 && addTarList.Count > 0)
        {
            var buffID = Config.ParamList[2].ToInt();
            var count = Config.ParamList[3].ToInt();
            var buffParam = new List<float>();
            for (var i = 4; i < Config.ParamList.Count; i++)
            {
                buffParam.Add(Config.ParamList[i]);
            }

            BattleBuffManager.AddBuff(addSpellList[0], buffID, addTarList[0], count, buffParam);
        }
    }
}