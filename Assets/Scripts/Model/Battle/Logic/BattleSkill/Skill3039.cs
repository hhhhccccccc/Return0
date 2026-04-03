using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3039 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        if (CheckBuffTypeCount(Subject, BuffType.Abnormal, 0, DataRelation.DaYu))
        {
            return 1;
        }
        
        return 0;
    }
    
    //施加至多2个自身持有的异常状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var buffList = Subject.GetRandomBuffByType(BuffType.Abnormal, 2);
        foreach (var buff in buffList)
        {
            DoAddBuff(Target, buff.BuffID, Subject, buff.LayerCount, buff.ParamList, BattleMomentType.ReleaseSkillAction);
        }
    }
}