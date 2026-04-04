using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3079 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }
    
    //回合内受到的全部伤害在下个回合开始时才会生效
    public override void DoDesitionAction(bool isPreDesition)
    {
        Subject.SetAccumulateDamage();
    }

    //获得3层武增状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    //清除全部兽化身状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoClearBuff(Subject, GameConst.Battle.BuffShouHuaShen);
    }
}