using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1033 : BattleSkillBase
{
    //todo 持有猊煞状态可使用
    
    //获得2层巧增
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffQiaoZeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //todo 玄炁+40，下回合猊煞状态不会产生消耗
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 40, BattleSource.Skill);
        DoAddBuff(Subject, 90008, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}