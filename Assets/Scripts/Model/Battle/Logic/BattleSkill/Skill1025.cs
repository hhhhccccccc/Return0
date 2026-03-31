using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1025 : BattleSkillBase
{
    //获得10层缓速和10层迅速
    public override void SelfActionWheelStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
    }

    //获得5层术衰状态和5层术增和5层技增和5层技衰
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffShuShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    //获得5个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}