using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4009 : BattleSkillBase
{
    //todo 下个回合开始获得1次行动次数
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, 90014, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //刚炁+15
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 15, BattleSource.Skill);
    }
}