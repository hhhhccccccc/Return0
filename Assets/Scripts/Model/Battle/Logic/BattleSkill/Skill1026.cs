using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1026 : BattleSkillBase
{
    //补充随机的键到达持有上限
    public override void SelfActionWheelStart()
    {
        DoAddRandomKeyToDefineCount(Subject, 0, ChangeKeyReason.SkillEffect);
    }

    //获得1次行动次数，下回合开始消耗全部键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
        DoAddBuff(Subject, 90004, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}