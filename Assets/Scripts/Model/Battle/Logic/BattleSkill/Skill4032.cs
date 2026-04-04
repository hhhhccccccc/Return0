using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4032 : BattleSkillBase
{
    //todo 本次行动不影响状态的存续
    
    //获得1次行动次数
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddActionTimes(Subject, 1);
    }

    //若持有的键低于3则随机获得键至5个
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var count = Subject.GetAllKeyCount();
        if (count < 3)
        {
            DoAddRandomKeyToDefineCount(Subject, 5, ChangeKeyReason.SkillEffect);
        }
    }

    //玄炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}