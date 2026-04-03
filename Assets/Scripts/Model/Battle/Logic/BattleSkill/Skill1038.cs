using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1038 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
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
    // 效果: 清除2个负面状态并获得10层避殃状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearBuffByType(Subject, BuffType.Abnormal, 2);
        DoAddBuff(Subject, GameConst.Battle.BuffBiYang, Subject, 10, null, BattleMomentType.ReleaseSkillAction);
    }
}