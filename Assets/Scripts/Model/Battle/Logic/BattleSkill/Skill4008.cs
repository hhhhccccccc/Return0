using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4008 : BattleSkillBase
{
    //行动加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 1);
    }

    //玄炁+32
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 32, BattleSource.Skill);
    }
    
    //todo 根据本次行动招式的二号键方向将自身卦位改变为对应阴卦直到下回合（↑巽↓坤←艮→离）
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        
    }
}