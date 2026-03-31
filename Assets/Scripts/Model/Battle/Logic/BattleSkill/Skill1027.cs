using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1027 : BattleSkillBase
{
    protected override int DontBeCounter(MomentParamModel paramModel)
    {
        return 6;
    }
    
    //若与杀式交锋则刚炁+20,若且互为目标则消耗目标20刚炁
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            
            if (otherUnit != null && CheckSkillIsKillingStyle(otherUnit, true))
            {
                DoChangeProperty(otherUnit, BattlePropertyType.GangQi, -20, BattleSource.Skill);
            }
            
            if (CheckMutualGoal(Subject, otherUnit))
            {
                DoChangeProperty(Subject, BattlePropertyType.GangQi, 20, BattleSource.Skill);
            }
        }
    }
}