using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2024 : BattleSkillBase
{
    protected override int DontBeCounter(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            if (CheckMutualGoal(Subject, target) && !BattleBuffManager.CheckTargetHasDownSkillBuff(target.EntityID))
            {
                return 1;
            }
        }
        return 0;
    }

    //施加3层刚屏合3层玄屏
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffGangPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}

