using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1052 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //将自身即将受到的敌方行动转移至目标友方
        // TODO: ChangeNearlyBeActionTargetToTeamOther
    }

    //刚炁+5，玄炁+5
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 5, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 5, BattleSource.Skill);
    }
}