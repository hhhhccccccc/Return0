using System.Collections.Generic;
using Zenject;

public class Skill3075 : BattleSkillBase
{
    //清除全部武增状态，若处于兽化身状态则不会清除武增状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        if (!Subject.HasBuff(GameConst.Battle.BuffShouHuaShen))
        {
            DoClearBuff(Subject, GameConst.Battle.BuffWuZeng);
        }
    }
}