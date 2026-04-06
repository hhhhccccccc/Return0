using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10141 : BattleBuffBase
{
    /// <summary>
    /// 武杀式直接造成伤害时根据行动加快效果提升伤害共{[int]}%，行动加快{[int]}息（行动每加快1息伤害提升5%，每层使行动加快1息）
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    protected override float OnAddDamagePct(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling)
            {
                var effectCount = Subject.BattleMomentManager.GetChangeActionWheel();
                return Math.Max(effectCount * GetConfigParamFloat(1), 0);;
            }
        }

        return 0;
    }

    protected override int OnGetChangeActionWheel()
    {
        return LayerCount * GetConfigParamInt(0);
    }
}
