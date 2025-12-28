using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10141 : BattleBuffBase
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    protected override float OnAddSkillDamageRate(MomentParamModel paramModel)
    {
        var effectCount = Subject.BattleChangeModelManager.GetChangeActionWheel();
        return Math.Max(effectCount * Config.ParamEx[1], 0);;
    }

    protected override int OnGetChangeActionWheel()
    {
        return LayerCount * Config.ParamEx[0].ToInt();
    }
}
