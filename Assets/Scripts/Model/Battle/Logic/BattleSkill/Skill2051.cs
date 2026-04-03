using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2051 : BattleSkillBase
{
    //行动加快1息或延迟1息，招式的玄炁消耗转为当前70%，至多70
    //todo  重新在敌手中随机选择行动目标
    public override void DoDesitionAction(bool isPreDesition)
    {
        var changeWheel = Util.GetRandomBool() ? 1 : -1;
        DoChangeActionWheel(Subject, changeWheel);
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.7f, 70);
    }

    //施加1层过劲状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
       DoAddBuff(Target, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}