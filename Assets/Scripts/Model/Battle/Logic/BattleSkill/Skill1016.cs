using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1016 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        if (CheckKeyCount(Subject, 5, DataRelation.DaYuDengYu))
        {
            return 1;
        }
        return 0;
    }
    //获得2层武增状态和3层力增状态和4层刚聚状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffGangJu, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

    //获得5个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}