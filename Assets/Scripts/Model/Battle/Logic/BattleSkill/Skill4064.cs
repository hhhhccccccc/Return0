using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4064 : BattleSkillBase
{
    //行动延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -2);
    }

    //扣除全部毒瘴状态向全部敌手施加1层毒瘴状态与2层伤口状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        Subject.ClearBuff(GameConst.Battle.BuffDuZhang);
        var oppoList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var unit in oppoList)
        {
            DoAddBuff(unit, GameConst.Battle.BuffDuZhang, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
            DoAddBuff(unit, GameConst.Battle.BuffShangKou, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}