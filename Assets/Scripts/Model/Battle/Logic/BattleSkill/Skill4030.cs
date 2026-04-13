using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4030 : BattleSkillBase
{
    //本次行动延迟2息，获得1层藏身状态和1层隐魂状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -2);
        DoAddBuff(Subject, GameConst.Battle.BuffCangShen, Subject, 1, null, BattleMomentType.DoDesitionAction);
        DoAddBuff(Subject, GameConst.Battle.BuffYinHun, Subject, 1, null, BattleMomentType.DoDesitionAction);
    }

    //行动的前2息内即将受到攻击将立即执行本次行动
    public override void BeforeUnderAction()
    {
        if (CheckBeActionInBeforeActionWheel(Subject, 2, false))
        {
            DoSetActionWheelToNow(Subject);
        }
    }

    //施加4层缓速状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffHuanSu, Subject, 4, null, BattleMomentType.BeforeClash);
    }

    //随机获得2个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 2, ChangeKeyReason.SkillEffect);
    }

    //获得1次行动次数
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
    }
}