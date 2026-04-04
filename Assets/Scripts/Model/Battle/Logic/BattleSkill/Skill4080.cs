using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4080 : BattleSkillBase
{
    //目标刚炁+5，玄炁+5，施加3层回春状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Target, BattlePropertyType.GangQi, 5, BattleSource.Skill);
        DoChangeProperty(Target, BattlePropertyType.XuanQi, 5, BattleSource.Skill); 
        DoAddBuff(Target, GameConst.Battle.BuffHuiChun, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}