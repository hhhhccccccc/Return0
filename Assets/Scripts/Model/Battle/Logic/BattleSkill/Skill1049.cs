using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1049 : BattleSkillBase
{
    //清除1个异常状态，获得5层玄聚5层刚聚和3层迅速
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        DoClearBuffByType(Subject, BuffType.Abnormal, 1);
        DoAddBuff(Subject, GameConst.Battle.BuffXuanJu, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffGangJu, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}