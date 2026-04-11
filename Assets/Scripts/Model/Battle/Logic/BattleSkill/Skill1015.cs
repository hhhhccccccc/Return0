using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1015 : BattleSkillBase
{
    //获得3层心眼状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }

    //刚炁+60
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 60, BattleSource.Skill);
    }

    //获得1层武增和2层力增
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 1, null, BattleMomentType.AfterAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 2, null, BattleMomentType.AfterAction);
    }
}