using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2053 : BattleSkillBase
{
    //若处于祖化身状态刚炁消耗变为玄炁消耗
    
    
    //todo 下一次行动使用招式所需的玄炁消耗变为等量的刚炁消耗
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, 72054, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}