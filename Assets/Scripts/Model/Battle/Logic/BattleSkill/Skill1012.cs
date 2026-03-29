using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1012 : BattleSkillBase
{
    //获得100%力的血炁甲状态层数，玄炁+5
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var power = Subject.GetProperty(BattlePropertyType.Power);
        DoAddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, (int)power, null, BattleMomentType.ReleaseSkillAction);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 5, BattleSource.Skill);
    }

    //todo 下次行动前受到伤害后减少等量体获得等量血炁甲层数 buff 行动后上这个buff
}