using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1005 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    
    //若与杀式交锋则敌手因招式效果获得的炁-100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (CheckSkillIsKillingStyle(otherUnit, true))
            {
                DoReduceHealQi(otherUnit, BattleMomentType.BeforeClash);
            }
        }
    }

    //获得1个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}