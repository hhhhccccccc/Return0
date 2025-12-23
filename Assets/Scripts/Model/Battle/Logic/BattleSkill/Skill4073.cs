using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4073 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        BattleBuffManager.AddBuff(Target, 74073, Subject, 2, new List<float>
        {   
            Config.ParamEx[0], Config.ParamEx[1]
        }, BattleMomentType.ReleaseSkillAction);
        
        BattleBuffManager.AddBuff(Subject, 74073, Subject, 2, new List<float>
        {   
            Config.ParamEx[2], Config.ParamEx[3]
        }, BattleMomentType.ReleaseSkillAction);
    }
}