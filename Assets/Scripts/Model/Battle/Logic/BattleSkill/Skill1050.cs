using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1050 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }
    
    //行动延迟3
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -3);
    }

    //若与杀式交锋则敌手因招式效果获得的炁-100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (CheckSkillIsKillingStyle(otherUnit, true))
            {
                DoReduceHealQi(otherUnit, BattleMomentType.BeforeClash);
            }
        }
    }

    //获得3个随机的键
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 3, ChangeKeyReason.SkillEffect);
    }

    //行动期间受到的伤害减少50%
    public override float ReduceDamagePct(int attackID, DamageType damageType)
    {
        if (IsInAction)
        {
            return 0.5f;
        }

        return 0;
    }
}