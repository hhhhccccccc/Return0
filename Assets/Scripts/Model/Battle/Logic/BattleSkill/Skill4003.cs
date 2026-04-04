using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4003 : BattleSkillBase
{
    //行动延迟1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -1);
    }

    //清除自身全部异常状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearBuffByType(Subject, BuffType.Abnormal, 0);
    }

    //获得2个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 2, ChangeKeyReason.SkillEffect);
    }
    
    //todo 不影响状态的续存
  
}