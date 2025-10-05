using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveKey : BattleMomentEffect
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
                for (int i = 1; i < Config.ParamList.Count; i++)
                {
                    var key = (BattleKeyType)Config.ParamList[i].ToInt();
                    target.ChangeKey(key, -1);
                }
            }
        }
    }
}