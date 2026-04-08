using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3020 : BattleSkillBase
{
    //若互为目标则本次行动的力倍率按照目标杀式的基础威力50%提高

    private float AddWelly;

    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        if (CheckMutualGoal(Subject, clashUnit))
        {
            var clashUnitSkill = clashUnit.GetSkill();
            if (clashUnitSkill != null)
            {
                var id = clashUnitSkill.SkillID;
                AddWelly = ConfigManager.GetBattleSkillConfig(id).WellyRateBase * 0.5f;
            }
        }
    }
    
    public override float GetWellyRateEx(int skillGuid)
    {
        return AddWelly;
    }

    public override void ClearTempData()
    {
        AddWelly = 0;
    }

    protected override void OnSkillRecycle()
    {
        AddWelly = 0;
    }
}