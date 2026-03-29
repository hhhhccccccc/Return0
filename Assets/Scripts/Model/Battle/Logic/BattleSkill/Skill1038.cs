using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1038 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 若与杀式交锋则敌手因招式效果获得的炁-100
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

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 清除2个负面状态并获得10层避殃状态
        DoClearBuffByType(Subject, BuffType.Abnormal, 2);
        DoAddBuff(Subject, GameConst.Battle.BuffBiYang, Subject, 10, null, BattleMomentType.ReleaseSkillAction);
    }
}