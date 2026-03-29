using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1006 : BattleSkillBase
{
    //招式的招式的刚炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillGangQiCost(Subject, 0.5f, 50);
    }

    //获得80%力的护体
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var power = Subject.GetProperty(BattlePropertyType.Power);
        DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (int)(power * 0.8f), null, BattleMomentType.AfterAction);
    }

    //todo 玄炁+10，下个回合开始获得本回合结束时等量的护体
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}