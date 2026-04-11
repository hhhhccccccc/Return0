using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3033 : BattleSkillBase
{
    //施加2层伤口
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffShangKou, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //玄炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}