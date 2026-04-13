using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3041 : BattleSkillBase
{
    //施加3层缓速和3层刚屏
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
         DoAddBuff(Target, GameConst.Battle.BuffHuanSu, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
         DoAddBuff(Target, GameConst.Battle.BuffGangPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    //玄炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}