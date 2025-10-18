using System;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect3072002 : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var gangQi = target.GetProperty(BattlePropertyType.GangQi);
                var xuanQi = target.GetProperty(BattlePropertyType.XuanQi);
                if (gangQi >= xuanQi)
                {
                    var cost = gangQi * Config.ParamList[1];
                    cost = Math.Min(cost, Config.ParamList[2]);
                    target.ChangeProperty(BattlePropertyType.GangQi, cost);
                }
                else
                {
                    var cost = xuanQi * Config.ParamList[1];
                    cost = Math.Min(cost, Config.ParamList[2]);
                    target.ChangeProperty(BattlePropertyType.XuanQi, cost);
                }
            }
        }
    }
}