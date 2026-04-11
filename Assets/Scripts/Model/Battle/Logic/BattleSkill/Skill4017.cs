using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4017 : BattleSkillBase
{
    //todo 若目标习有心法：勾牌人则将行动所在息调整为同一息
    public override void DoDesitionAction(bool isPreDesition)
    {
        
    }

    //todo 若目标习有心法：勾牌人则使本回合其获得1次行动次数
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        
    }

    //目标刚炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 10, BattleSource.Skill);
    }
}