using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2045 : BattleSkillBase
{
    private bool CanAddWelly { get; set; }
    
    //玄炁大于目标刚炁则威力增加10百分比
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (CheckPropertyCompare(Subject, BattlePropertyType.XuanQi, otherUnit, BattlePropertyType.GangQi,
                    DataRelation.DaYu))
            {
                CanAddWelly = true;
            }
        }
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        if (CanAddWelly)
        {
            return 0.1f;
        }

        return 0;
    }

    //刚炁+20
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 20, BattleSource.Skill);
    }

    public override void ClearTempData()
    {
        CanAddWelly = false;
    }

    protected override void OnSkillRecycle()
    {
        CanAddWelly = false;
    }
} 