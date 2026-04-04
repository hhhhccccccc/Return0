using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4059 : BattleSkillBase
{
    //自身与目标获得3层武增状态和2层雷行
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLeiXing, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffWuZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffLeiXing, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //todo 自身与目标下个回合开始获得本回结束时等量的力增状态
}