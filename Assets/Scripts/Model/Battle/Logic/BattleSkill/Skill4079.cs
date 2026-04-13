using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4079 : BattleSkillBase
{
    //若目标行动已揭示,为杀式则施加1层盲目状态，非杀式则施加3层玄屏刚屏
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (Target.IsBeActionReveals)
        {
            if (CheckSkillIsKillingStyle(Target, true))
            {
                DoAddBuff(Target, GameConst.Battle.BuffMangMu, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
            }
            else
            {
                DoAddBuff(Target, GameConst.Battle.BuffGangPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
                DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
            }
        } 
    }

    //获得4个随机的键
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 4, ChangeKeyReason.SkillEffect);
    }
}