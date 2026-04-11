using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4078 : BattleSkillBase
{
    //todo 3息内全部的角色无法恢复刚炁，且行动的敌手获得4层赤沸状态
    
    
    //刚炁+50
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 50, BattleSource.Skill);
    }
}