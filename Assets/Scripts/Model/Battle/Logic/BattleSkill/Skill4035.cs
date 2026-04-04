using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4035 : BattleSkillBase
{
    //本次行动加快2息，在平、彻、起三种状态中随机获得一种
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 2);
        DoAddPoolBuffByCount(Subject, 1, 200006, BattleMomentType.DoDesitionAction);
    }
    
    //todo 获得5层墨痕状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        
    }
}