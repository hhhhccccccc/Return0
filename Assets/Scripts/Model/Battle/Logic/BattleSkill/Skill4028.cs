using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4028 : BattleSkillBase
{
    //获得3层回避状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }

    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }
    
    //清除自身回避状态，获得1层匿形状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearBuff(Subject, GameConst.Battle.BuffHuiBi);
        DoAddBuff(Subject, GameConst.Battle.BuffNiXing, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}