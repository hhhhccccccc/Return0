using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_AddBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var addSpell = GetUnitByParamID(Config.ParamList[0]);
        var addTar = GetUnitByParamID(Config.ParamList[1]);
        if (addSpell != null && addTar != null)
        {
            var buffID = Config.ParamList[2].ToInt();
            var count = Config.ParamList[3].ToInt();
            var buffParam = new List<float>();
            for (var i = 4; i < Config.ParamList.Count; i++)
            {
                buffParam.Add(Config.ParamList[i]);
            }

            BattleBuffManager.AddBuff(addSpell, buffID, addTar, count, buffParam);
        }
    }
}