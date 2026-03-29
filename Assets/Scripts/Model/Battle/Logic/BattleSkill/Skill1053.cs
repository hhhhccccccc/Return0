using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1053 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //获得3层武增状态和3层力增状态
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        //防+10，力+30
        DoChangeProperty(Subject, BattlePropertyType.DefendInt, 10);
        DoChangeProperty(Subject, BattlePropertyType.PowerInt, 30);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        //获得50%力层的护体状态
        var power = Subject.GetProperty(BattlePropertyType.Power);
        DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (int)(power * 0.5f), null, BattleMomentType.AfterAction);
    }

}