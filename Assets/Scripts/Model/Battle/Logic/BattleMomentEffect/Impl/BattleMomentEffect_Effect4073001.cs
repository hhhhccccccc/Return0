using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect4073001 : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        for (int i = 0; i < Config.ParamList.Count; i+=3)
        {
            var targetList = GetUnitByParamID(Config.ParamList[i + 0]);
            if (targetList.Count > 0)
            {
                foreach (var target in targetList)
                {
                    BattleBuffManager.AddBuff(target, 74073, Subject, 2, new List<float>
                    {   
                        Config.ParamList[i + 1], Config.ParamList[i + 2]
                    }, MomentType);
                }
            }
        }
    }
}