using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1050 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        //行动延迟3息
        DoChangeActionWheel(Subject, -3);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        //若与杀式交锋则敌手因招式效果获得的炁-100
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
            }
        }
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        //获得3个随机的键
        DoAddRandomKey(Subject, 3, ChangeKeyReason.SkillEffect);
    }
}