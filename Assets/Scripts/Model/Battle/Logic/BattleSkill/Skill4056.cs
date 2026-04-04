using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4056 : BattleSkillBase
{
    //若持有刚炁大于玄炁则玄炁+25并获得2层武增状态，若持有刚炁小于玄炁则刚炁+25并获得2层术增状态，持有刚炁与玄炁持平则获得5层武增和5层玄增状态和5层巧增状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var gangQiInt = Subject.GetProperty(BattlePropertyType.GangQi).ToInt();
        var xuanQiInt = Subject.GetProperty(BattlePropertyType.XuanQi).ToInt();
        if (gangQiInt > xuanQiInt)
        {
            DoChangeProperty(Subject, BattlePropertyType.XuanQi, 25, BattleSource.Skill);
            DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        }
        else if (gangQiInt == xuanQiInt)
        {
            DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
            DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
            DoAddBuff(Subject, GameConst.Battle.BuffQiaoZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        }
        else
        {
            DoChangeProperty(Subject, BattlePropertyType.GangQi, 25, BattleSource.Skill);
            DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}