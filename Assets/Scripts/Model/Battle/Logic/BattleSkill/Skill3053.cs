using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3053 : BattleSkillBase
{
    //施加1层伤口状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffShangKou, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //玄炁+25
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 25, BattleSource.Skill);
    }
}