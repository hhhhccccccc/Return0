using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1001 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }

    //行动的前2息内即将受到攻击将立即执行本次行动
    public override void BeforeUnderAction()
    {
        if (CheckBeActionInBeforeActionWheel(Subject, 2, false))
        {
            DoSetActionWheelToNow(Subject);
        }
    }
    
    //若与杀式交锋则敌手因招式效果获得的炁-100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                if (CheckSkillIsKillingStyle(otherUnit, true))
                {
                    DoReduceHealQi(otherUnit, BattleMomentType.BeforeClash);
                }
            }
        }
    }

    //获得1个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}